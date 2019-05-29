using System;
using System.Collections.Generic;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.ProjectileHelpers;
using HamstarHelpers.Services.Timers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;


namespace DestructibleTiles {
	partial class DestructibleTilesProjectile : GlobalProjectile {
		private static object MyLock = new object();



		////////////////

		public static IDictionary<string, Tuple<int, int>> GetExplosives() {
			var projectiles = new Dictionary<string, Tuple<int, int>>();

			for( int i = 0; i < Main.projectileTexture.Length; i++ ) {
				var proj = new Projectile();
				Main.projectile[0] = proj;

				try {
					proj.SetDefaults( i );

					if( proj.aiStyle == 16 ) {
						int damage = proj.damage;

						proj.position = new Vector2( 3000, 1000 );
						proj.owner = Main.myPlayer;
						proj.hostile = true;

						proj.timeLeft = 3;
						proj.VanillaAI();

						int radius = ( proj.width + proj.height ) / 4;
						damage = damage > proj.damage ? damage : proj.damage;
						
						string projName = ProjectileIdentityHelpers.GetProperUniqueId( i );
						projectiles[projName] = Tuple.Create( radius, damage );
					}

					Main.projectile[proj.whoAmI] = new Projectile();
				} catch {
					continue;
				}
			}

			return projectiles;
		}



		////////////////

		public override void Kill( Projectile projectile, int timeLeft ) {
			if( timeLeft > 3 ) { return; }

			var mymod = DestructibleTilesMod.Instance;
			string projName = ProjectileIdentityHelpers.GetProperUniqueId( projectile.type );
			bool isExplosive = mymod.Config.ProjectilesAsExplosivesAndRadius.ContainsKey( projName );

			if( isExplosive ) {
				int tileX = (int)projectile.position.X >> 4;
				int tileY = (int)projectile.position.Y >> 4;
				int radius = mymod.Config.ProjectilesAsExplosivesAndRadius[ projName ];
				int damage = DestructibleTilesProjectile.ComputeProjectileDamage( projectile );

				if( mymod.Config.DebugModeInfo ) {
					Main.NewText( "RADIUS - " + projectile.Name + "("+projName+"), radius:" + radius + ", damage:"+damage );
				}

				DestructibleTilesProjectile.HitTilesInRadius( tileX, tileY, radius, damage );
			}
		}

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
				if( !isExplosive ) {
					var rect = new Rectangle( (int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height );
					rect.X += (int)oldVelocity.X;
					rect.Y += (int)oldVelocity.Y;

					bool onlySometimesRespects;
					bool respectsPlatforms = Helpers.ProjectileHelpers.ProjectileHelpers.VanillaProjectileRespectsPlatforms( projectile, out onlySometimesRespects )
						&& !onlySometimesRespects;

					IDictionary<int, int> hits = Helpers.TileHelpers.TileFinderHelpers.GetSolidTilesInWorldRectangle( rect, respectsPlatforms, false );
					int damage = DestructibleTilesProjectile.ComputeProjectileDamage( projectile );

					if( mymod.Config.DebugModeInfo ) {
						Main.NewText( "RECTANGLE - " + projectile.Name + " hits #" + hits.Count + " tiles" );
					}

					DestructibleTilesProjectile.HitTilesInSet( damage, hits );
				}
			}

			return base.OnTileCollide( projectile, oldVelocity );
		}
	}
}
