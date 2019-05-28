using HamstarHelpers.Helpers.ProjectileHelpers;
using System;
using Terraria;
using Terraria.ModLoader;


namespace DestructibleTiles {
	partial class DestructibleTilesProjectile : GlobalProjectile {
		public static int ComputeProjectileDamage( Projectile projectile ) {
			var mymod = DestructibleTilesMod.Instance;
			string projName = ProjectileIdentityHelpers.GetProperUniqueId( projectile.type );

			if( mymod.Config.ProjectileTileDamageOverrides.ContainsKey( projName ) ) {
				return mymod.Config.ProjectileTileDamageOverrides[projName];
			}

			if( projectile.damage > 0 ) {
				return (int)((float)projectile.damage * mymod.Config.AllDamagesScale);
			}

			if( mymod.Config.ProjectileTileDamageDefaults.ContainsKey( projName ) ) {
				return (int)((float)mymod.Config.ProjectileTileDamageDefaults[projName] * mymod.Config.AllDamagesScale);
			}

			return (int)((float)projectile.damage * mymod.Config.AllDamagesScale );
		}

		public static float ComputeHitDamage( Tile tile, int baseDamage, int totalHits ) {
			var mymod = DestructibleTilesMod.Instance;
			float dmg = (float)baseDamage / (float)totalHits;
			float scale = 1f;
			string tileName = Helpers.TileHelpers.TileIdentityHelpers.GetProperUniqueId( tile );

			float armor = mymod.Config.TileArmor.ContainsKey(tileName) ? mymod.Config.TileArmor[ tileName ] : 0f;
			if( armor >= dmg ) {
				return 0;
			}
			
			if( mymod.Config.TileDamageScaleOverrides.ContainsKey(tileName) ) {
				scale = mymod.Config.TileDamageScaleOverrides[ tileName ];
			}

			if( mymod.Config.UseVanillaTileDamageScalesUnlessOverridden ) {
				bool isAbsolute;
				scale *= Helpers.TileHelpers.TileHelpers.GetDamageScale( tile, dmg, out isAbsolute );

				if( isAbsolute ) {
					return 100f;	// max
				}
			}

			return (dmg - armor) * scale;
		}
	}
}
