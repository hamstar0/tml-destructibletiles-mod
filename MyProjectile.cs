using System;
using System.Collections.Generic;
using HamstarHelpers.Helpers.ProjectileHelpers;
using HamstarHelpers.Services.Timers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;


namespace DestructibleTiles {
	partial class DestructibleTilesProjectile : GlobalProjectile {
		private static object MyLock = new object();



		////////////////
		
		public override bool OnTileCollide( Projectile projectile, Vector2 oldVelocity ) {
			var mymod = DestructibleTilesMod.Instance;

			//float avg = (projectile.width + projectile.height) / 2;
			//bool isOblong = Math.Abs( 1 - (projectile.width / avg) ) > 0.2f;
			string projName = ProjectileIdentityHelpers.GetProperUniqueId( projectile.type );
			bool isExplosive = mymod.Config.ProjectilesAsExplosivesAndRadius.ContainsKey( projName );

			string timerName = "PTH_" + projectile.whoAmI;
			bool isConsecutive = Timers.GetTimerTickDuration( timerName ) > 0;
			Timers.SetTimer( timerName, 2, () => false );

			if( !isConsecutive ) {
				if( isExplosive ) {
					if( mymod.Config.DebugModeInfo ) {
						Main.NewText( "RADIUS - " + projectile.Name + " hits with radius "
							+ ( ( (float)projectile.width + (float)projectile.height ) * 0.5f ).ToString( "N2" ) );
					}

					this.HitTilesInRadius( projectile, 8 );
				} else {
					var rect = new Rectangle( (int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height );
					rect.X += (int)oldVelocity.X;
					rect.Y += (int)oldVelocity.Y;

					bool onlySometimesRespects;
					bool respectsPlatforms = Helpers.ProjectileHelpers.ProjectileHelpers.VanillaProjectileRespectsPlatforms( projectile, out onlySometimesRespects )
						&& !onlySometimesRespects;

					IDictionary<int, int> hits = Helpers.TileHelpers.TileFinderHelpers.GetSolidTilesInWorldRectangle( rect, respectsPlatforms, false );

					if( mymod.Config.DebugModeInfo ) {
						Main.NewText( "RECTANGLE - " + projectile.Name + " hits #" + hits.Count + " tiles" );
					}

					this.HitTilesInSet( projectile, hits );
				}
			}

			return base.OnTileCollide( projectile, oldVelocity );
		}
	}
}
