using System;
using Terraria;
using Terraria.ModLoader;


namespace DestructibleTiles.Helpers.TileHelpers {
	public class TileIdentityHelpers {
		public static string GetProperUniqueId( int tileId ) {
			ModTile modTile = TileLoader.GetTile( tileId );

			if( modTile == null ) {
				return "Terraria." + tileId;
			}

			if( modTile.Name == "" ) {
				return modTile.mod.Name + "." + modTile.GetType().Name;
			} else {
				return modTile.mod.Name + "." + modTile.Name;
			}
		}

		public static string GetProperUniqueId( Tile tile ) {
			return TileIdentityHelpers.GetProperUniqueId( tile.type );
		}
	}
}
