using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace DestructibleTiles {
	partial class DestructibleTilesProjectile : GlobalProjectile {
		public static IDictionary<string, Tuple<int, int>> GetExplosives() {
			var projectiles = new Dictionary<string, Tuple<int, int>>();
			int inactivePos = 0;

			for( int i = 0; i < Main.projectile.Length; i++ ) {
				if( Main.projectile[i] == null || !Main.projectile[i].active ) {
					inactivePos = i;
					break;
				}
			}

			for( int i = 0; i < Main.projectileTexture.Length; i++ ) {
				var proj = new Projectile();
				Main.projectile[inactivePos] = proj;

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
						
						string projName = ProjectileID.GetUniqueKey( i );
						projectiles[projName] = Tuple.Create( radius, damage );
					}
				} catch {
					continue;
				}
			}

			Main.projectile[inactivePos] = new Projectile();

			return projectiles;
		}
	}
}
