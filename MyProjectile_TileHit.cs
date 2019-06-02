﻿using System;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.TileHelpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace DestructibleTiles {
	partial class DestructibleTilesProjectile : GlobalProjectile {
		public static bool HitTile( int damage, int tileX, int tileY, int totalHits, float percent = 1f ) {
			var mymod = DestructibleTilesMod.Instance;
			Tile tile = Main.tile[tileX, tileY];
			//HitTile plrTileHits = Main.LocalPlayer.hitTile;

			//int tileHitId = plrTileHits.HitObject( tileX, tileY, 1 );
			int dmg = (int)( DestructibleTilesProjectile.ComputeHitDamage( tile, damage, totalHits ) * percent );

			if( mymod.Config.DebugModeInfo ) {
				Main.NewText( TileIdentityHelpers.GetVanillaTileName( tile.type ) + " hit for " + dmg.ToString( "N2" ) );
			}

			if( mymod.TileDataMngr.AddDamage( tileX, tileY, dmg ) >= 100 ) {//
			//if( plrTileHits.AddDamage( tileHitId, dmg, true ) >= 100 ) {
			//	plrTileHits.Clear( tileHitId );
				WorldGen.KillTile( tileX, tileY, false, false, !mymod.Config.DestroyedTilesDropItems );
				if( Main.netMode == 1 ) {
					int itemDropMode = mymod.Config.DestroyedTilesDropItems ? 0 : 4;

					NetMessage.SendData( MessageID.TileChange, -1, -1, null, itemDropMode, (float)tileX, (float)tileY, 0f, 0, 0, 0 );
				}

				Helpers.ParticleHelpers.ParticleFxHelpers.MakeDustCloud( new Vector2((tileX * 16) + 8, (tileY * 16) + 8), 1, 1f, 1.2f );

				return true;
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
					if( TileHelpers.IsAir(Main.tile[i, j]) ) { continue; }

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
