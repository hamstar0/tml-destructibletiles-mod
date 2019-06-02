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
				if( i<0 || i>Main.maxTilesX-1 ) { continue; }

				for( int j = (worldRect.Y >> 4); (j << 4) <= projBottom; j++ ) {
					if( j < 0 || j > Main.maxTilesY - 1 ) { continue; }

					Tile tile = Main.tile[i, j];

					if( HamstarHelpers.Helpers.TileHelpers.TileHelpers.IsAir( tile ) ) { continue; }
					if( !HamstarHelpers.Helpers.TileHelpers.TileHelpers.IsSolid( tile, includesPlatforms, includesActuatedTiles ) ) { continue; }

					hits[i] = j;
				}
			}

			return hits;
		}


		public static Point? GetNearestSolidTile( Vector2 worldPos, int maxRadius = Int32.MaxValue,
				bool isPlatformSolid = false, bool isActuatedSolid = false ) {
			int midX = (int)Math.Round( worldPos.X );
			int midY = (int)Math.Round( worldPos.Y );
			int tileX = midX >> 4;
			int tileY = midY >> 4;
			Tile tile = Main.tile[ tileX, tileY ];

			if( HamstarHelpers.Helpers.TileHelpers.TileHelpers.IsSolid( tile, isPlatformSolid, isActuatedSolid ) ) {
				return new Point( tileX, tileY );
			}

			int max = Math.Max( Main.maxTilesX - 1, Main.maxTilesY - 1 ) * 16;
			max = Math.Min( maxRadius, max );

			for( int radius = 16; radius < max; radius += 16 ) {
				double radMin = radius - 8;
				double radMax = radius + 8;
				radMin *= radMin;
				radMax *= radMax;

				for( double inX = -radius; inX <= radius; inX+=16 ) {
					for( double inY = -radius; inY <= radius; inY += 16 ) {
						double distSqr = (inX * inX) + (inY * inY);
						if( distSqr < radMin || distSqr > radMax ) {
							continue;
						}
						//double dist = Math.Sqrt( ( inX * inX ) + ( inY * inY ) );
						//if( dist < radMin || dist > radMax ) {
						//	continue;
						//}

						tileX = (midX + (int)inX) >> 4;
						if( tileX < 0 || tileX >= (Main.maxTilesX - 1) ) {
							continue;
						}

						tileY = (midY + (int)inY) >> 4;
						if( tileY < 0 || tileY >= (Main.maxTilesY - 1) ) {
							continue;
						}

						tile = Main.tile[ tileX, tileY ];

						if( HamstarHelpers.Helpers.TileHelpers.TileHelpers.IsSolid(tile, isPlatformSolid, isActuatedSolid) ) {
							return new Point( tileX, tileY );
						}
					}
				}
			}

			return null;
		}
	}
}
