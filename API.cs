using DestructibleTiles.MultiHitTile;
using HamstarHelpers.Helpers.DotNET.Extensions;
using Terraria;


namespace DestructibleTiles {
	public static partial class DestructibleTilesAPI {
		public static float ComputeDamage( int tileX, int tileY, int damage, int totalHits ) {
			if( !TileDataManager.IsValidTile( tileX, tileY ) ) {
				return 0f;
			}

			return DestructibleTilesProjectile.ComputeHitDamage( Framing.GetTileSafely(tileX, tileY), damage, totalHits );
		}

		////

		public static int GetTileHealth( int tileX, int tileY ) {
			if( !TileDataManager.IsValidTile( tileX, tileY ) ) {
				return -1;
			}

			var mymod = DestructibleTilesMod.Instance;
			TileData tileData = mymod.TileDataMngr.Data.Get2DOrDefault( tileX, tileY );
			if( tileData == null ) {
				return -1;
			}
			
			return 100 - tileData.Damage;
		}

		////

		public static bool DamageTile( int tileX, int tileY, int damage, int totalHits ) {
			return DestructibleTilesProjectile.HitTile( damage, tileX, tileY, totalHits );
		}

		public static void DamageTilesInRadius( int tileX, int tileY, int damage, int radius ) {
			DestructibleTilesProjectile.HitTilesInRadius( tileX, tileY, radius, damage );
		}
	}
}
