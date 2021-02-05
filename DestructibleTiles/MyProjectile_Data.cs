using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;


namespace DestructibleTiles {
	partial class DestructibleTilesProjectile : GlobalProjectile {
		public static IDictionary<ProjectileDefinition, (int radius, int damage)> GetExplosivesStats() {
			var projectiles = new Dictionary<ProjectileDefinition, (int, int)>();
			int inactivePos = 0;

			for( int i = 0; i < Main.projectile.Length; i++ ) {
				if( Main.projectile[i] == null || !Main.projectile[i].active ) {
					inactivePos = i;
					break;
				}
			}

			for( int i = 0; i < Main.projectileTexture.Length; i++ ) {
				(int, int)? stats = DestructibleTilesProjectile.CalculateExplosiveStats( inactivePos, i );

				if( stats.HasValue ) {
					var projDef = new ProjectileDefinition( i );
					projectiles[ projDef ] = stats.Value;
				}
			}

			Main.projectile[ inactivePos ] = new Projectile();

			return projectiles;
		}


		private static (int radius, int damage)? CalculateExplosiveStats( int inactivePos, int projId ) {
			var proj = new Projectile();
			Main.projectile[ inactivePos ] = proj;

			try {
				proj.SetDefaults( projId );

				if( proj.aiStyle == 16 ) {
					int damage = proj.damage;

					proj.position = new Vector2( 3000, 1000 );
					proj.owner = Main.myPlayer;
					proj.hostile = true;

					proj.timeLeft = 3;
					proj.VanillaAI();

					int radius = ( proj.width + proj.height ) / 4;
					damage = damage > proj.damage ? damage : proj.damage;

					return (radius, damage);
				}
			} catch { }

			return null;
		}
	}
}
