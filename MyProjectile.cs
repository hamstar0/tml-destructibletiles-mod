using System;
using System.Collections.Generic;
using HamstarHelpers.Helpers.TileHelpers;
using HamstarHelpers.Services.Timers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;


namespace DestructibleTiles {
	partial class DestructibleTilesProjectile : GlobalProjectile {
		private static object MyLock = new object();



		////////////////
		
		public override bool OnTileCollide( Projectile projectile, Vector2 oldVelocity ) {
			float avg = (projectile.width + projectile.height) / 2;
			bool isOblong = Math.Abs( 1 - (projectile.width / avg) ) > 0.2f;
			
			if( isOblong ) {
				var rect = new Rectangle( (int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height );
				rect.X += (int)oldVelocity.X;
				rect.Y += (int)oldVelocity.Y;

				bool onlySometimesRespects;
				bool respectsPlatforms = Helpers.ProjectileHelpers.ProjectileHelpers.VanillaProjectileRespectsPlatforms( projectile, out onlySometimesRespects )
					&& !onlySometimesRespects;

				IDictionary<int, int> hits = Helpers.TileHelpers.TileFinderHelpers.GetSolidTilesInWorldRectangle( rect, respectsPlatforms, false );

				lock( DestructibleTilesProjectile.MyLock ) {
					string timerName = "PTH_" + projectile.whoAmI;
					bool isConsecutive = Timers.GetTimerTickDuration( timerName ) > 0;
					Timers.SetTimer( timerName, 2, () => false );

					if( !isConsecutive ) {
						this.HitTilesInSet( projectile, hits );
					}
				}
			} else {
				f
			}

			return base.OnTileCollide( projectile, oldVelocity );
		}
	}
}
