using System;
using Terraria;
using Terraria.ModLoader;


namespace DestructibleTiles {
	partial class DestructibleTilesProjectile : GlobalProjectile {
		public static int ComputeHitDamage( Tile tile, Projectile projectile, int totalHits ) {
			var mymod = DestructibleTilesMod.Instance;
			float dmg = (float)projectile.damage / (float)totalHits;
			float scale = 1f;
			string tileName = Helpers.TileHelpers.TileIdentityHelpers.GetProperUniqueId( tile );

			if( mymod.Config.TileDamageScaleOverrides.ContainsKey(tileName) ) {
				scale = mymod.Config.TileDamageScaleOverrides[ tileName ];
			}

			if( mymod.Config.UseVanillaTileDamageScalesUnlessOverridden ) {
				bool isAbsolute;
				scale *= Helpers.TileHelpers.TileHelpers.GetDamageScale( tile, dmg, out isAbsolute );

				if( isAbsolute ) {
					return 100;	// max
				}
			}

			return (int)(dmg * scale);
		}
	}
}
