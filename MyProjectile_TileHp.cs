using HamstarHelpers.Components.DataStructures;
using System;
using Terraria;
using Terraria.ModLoader;


namespace DestructibleTiles {
	partial class DestructibleTilesProjectile : GlobalProjectile {
		public static float ComputeHitDamage( Tile tile, Projectile projectile, int totalHits ) {
			var mymod = DestructibleTilesMod.Instance;
			float dmg = (float)projectile.damage / (float)totalHits;
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

			return Math.Max( 0, (dmg - armor) * scale );
		}
	}
}
