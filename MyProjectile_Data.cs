using System;
using System.Collections.Generic;
using System.Linq;
using DestructibleTiles.Helpers.CollisionHelpers;
using DestructibleTiles.Helpers.TileHelpers;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.ProjectileHelpers;
using HamstarHelpers.Services.Timers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;


namespace DestructibleTiles {
	partial class DestructibleTilesProjectile : GlobalProjectile {
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
				} catch {
					continue;
				}
			}

			Main.projectile[0] = new Projectile();

			return projectiles;
		}
	}
}
