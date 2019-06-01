using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;


namespace DestructibleTiles.Helpers.CollisionHelpers {
	public class CollisionHelpers {
		public static float MeasureWorldDistanceToTile( Vector2 position, Vector2 direction, float maxDistance, List<Tuple<int, int>> ignoredTiles = null ) {
			int fromTileX = (int)position.X / 16;
			int fromTileY = (int)position.Y / 16;
			Vector2 from = position + direction * maxDistance;
			int toTileX = (int)from.X / 16;
			int toTileY = (int)from.Y / 16;

			Tuple<int, int> toTile;
			float dist;

			if( !Collision.TupleHitLine( fromTileX, fromTileY, toTileX, toTileY, 0, 0, ignoredTiles ?? new List<Tuple<int, int>>(), out toTile ) ) {
				dist = new Vector2( Math.Abs( fromTileX - toTile.Item1 ), Math.Abs( fromTileY - toTile.Item2 ) ).Length() * 16f;
			} else if( toTile.Item1 == toTileX && toTile.Item2 == toTileY ) {
				dist = maxDistance;
			} else {
				dist = new Vector2( Math.Abs(fromTileX - toTile.Item1), Math.Abs(fromTileY - toTile.Item2) ).Length() * 16f;
			}

			return dist;
		}
	}
}
