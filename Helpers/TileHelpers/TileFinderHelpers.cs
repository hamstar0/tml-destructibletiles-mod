using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;


namespace DestructibleTiles.Helpers.TileHelpers {
	public class TileFinderHelpers {
		public static IDictionary<int, int> GetSolidTilesInWorldRectangle( Rectangle worldRect, bool includesPlatforms, bool includesActuatedTiles ) {
			int projRight = worldRect.X + worldRect.Width;
			int projBottom = worldRect.Y + worldRect.Height;


			IDictionary<int, int> hits = new Dictionary<int, int>();

			for( int i = (worldRect.X >> 4); (i << 4) <= projRight; i++ ) {
				for( int j = (worldRect.Y >> 4); (j << 4) <= projBottom; j++ ) {
					Tile tile = Main.tile[i, j];
					if( HamstarHelpers.Helpers.TileHelpers.TileHelpers.IsAir( tile ) ) { continue; }
					if( !HamstarHelpers.Helpers.TileHelpers.TileHelpers.IsSolid( tile, includesPlatforms, includesActuatedTiles ) ) { continue; }

					hits[i] = j;
				}
			}

			return hits;
		}
	}
}
