using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Fx;
using HamstarHelpers.Helpers.Tiles;


namespace DestructibleTiles {
	partial class DestructibleTilesProjectile : GlobalProjectile {
		public static bool HitTile( int damage, int tileX, int tileY, int totalHits, float percent = 1f ) {
			var mymod = DestructibleTilesMod.Instance;
			var config = DestructibleTilesConfig.Instance;
			Tile tile = Framing.GetTileSafely( tileX, tileY );
			//HitTile plrTileHits = Main.LocalPlayer.hitTile;

			//int tileHitId = plrTileHits.HitObject( tileX, tileY, 1 );
			int dmg = (int)( DestructibleTilesProjectile.ComputeHitDamage( tile, damage, totalHits ) * percent );

			if( config.DebugModeInfo ) {
				Main.NewText( HamstarHelpers.Helpers.Tiles.Attributes.TileAttributeHelpers.GetVanillaTileDisplayName( tile.type ) + " hit for " + dmg.ToString( "N2" ) );
			}

			if( mymod.TileDataMngr.AddDamage( tileX, tileY, dmg ) >= 100 ) {//
			//if( plrTileHits.AddDamage( tileHitId, dmg, true ) >= 100 ) {
			//	plrTileHits.Clear( tileHitId );
				bool isTileKilled = TileHelpers.KillTileSynced(
					tileX: tileX,
					tileY: tileY,
					effectOnly: false,
					dropsItem: config.DestroyedTilesDropItems,
					forceSyncIfUnchanged: true,
					suppressErrors: false
				);

				if( isTileKilled ) {
					WorldGen.SquareTileFrame( tileX, tileY );   // unnecessary?

					ParticleFxHelpers.MakeDustCloud(
						new Vector2( (tileX * 16) + 8, (tileY * 16) + 8 ),
						1,
						0.3f,
						1.2f
					);
				}

				return isTileKilled;
			}

			return false;
		}

		
		////////////////
		
		public static void HitTilesInRadius( int tileX, int tileY, int radius, int damage ) {
			int radiusTiles = (int)Math.Round( (double)(radius / 16) );
			int radiusTilesSquared = radiusTiles * radiusTiles;

			int left = tileX - (radiusTiles + 1);
			int right = tileX + (radiusTiles + 1);
			int top = tileY - (radiusTiles + 1);
			int bottom = tileY + (radiusTiles + 1);

			for( int i=left; i<right; i++ ) {
				if( i < 0 || i >= Main.maxTilesX - 1 ) { continue; }

				for( int j=top; j<bottom; j++ ) {
					if( j < 0 || j >= Main.maxTilesY - 1 ) { continue; }
					if( TileHelpers.IsAir(Framing.GetTileSafely(i, j)) ) { continue; }

					int xOff = i - tileX;
					int yOff = j - tileY;

					int currTileDistSquared = (xOff * xOff) + (yOff * yOff);
					if( currTileDistSquared > radiusTilesSquared ) { continue; }

					float percentToCenter = 1f - ((float)currTileDistSquared / (float)radiusTilesSquared);

					DestructibleTilesProjectile.HitTile( damage, i, j, 1, percentToCenter );
				}
			}
		}
	}
}
