using System;
using System.Collections.Generic;
using HamstarHelpers.Helpers.TileHelpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace DestructibleTiles {
	partial class DestructibleTilesProjectile : GlobalProjectile {
		public static bool HitTile( int damage, int tileX, int tileY, int totalHits, float percent=1f ) {
			var mymod = DestructibleTilesMod.Instance;
			HitTile plrTileHits = Main.LocalPlayer.hitTile;
			
			int tileHitId = plrTileHits.HitObject( tileX, tileY, 1 );
			int dmg = (int)(DestructibleTilesProjectile.ComputeHitDamage( Main.tile[tileX, tileY], damage, totalHits ) * percent);

			if( mymod.Config.DebugModeInfo ) {
				Main.NewText( TileIdentityHelpers.GetVanillaTileName(Main.tile[tileX, tileY].type)+" hit for "+dmg.ToString("N2") );
			}

			if( plrTileHits.AddDamage(tileHitId, dmg, true) >= 100 ) {
				plrTileHits.Clear( tileHitId );

				WorldGen.KillTile( tileX, tileY, false, false, !mymod.Config.DestroyedTilesDropItems );
				if( Main.netMode == 1 ) {
					int itemDropMode = mymod.Config.DestroyedTilesDropItems ? 0 : 4;

					NetMessage.SendData( MessageID.TileChange, -1, -1, null, itemDropMode, (float)tileX, (float)tileY, 0f, 0, 0, 0 );
				}

				Helpers.ParticleHelpers.ParticleFxHelpers.MakeDustCloud( new Vector2((tileX*16) + 8, (tileY*16) + 8), 1, 1f, 1.2f );

				return true;
			}

			return false;
		}



		////////////////
		
		public void HitTilesInSet( int damage, IDictionary<int, int> hits ) {
			var mymod = DestructibleTilesMod.Instance;
			/*IOrderedEnumerable<KeyValuePair<int, int>> orderedHits;

			orderedHits = hits.OrderBy( ( kv ) => {
				var pos = new Vector2( kv.Key, kv.Value );
				return Vector2.DistanceSquared( pos, projectile.Center );
			} );*/

			foreach( var xy in hits ) {
				DestructibleTilesProjectile.HitTile( damage, xy.Key, xy.Value, hits.Count );
			}
		}


		public void HitTilesInRadius( int tileX, int tileY, int radius, int damage ) {
			int radiusTiles = (int)Math.Round( (double)(radius / 16) );
			int radiusTilesSquared = radiusTiles * radiusTiles;

			int left = tileX - (radiusTiles + 1);
			int right = tileX + (radiusTiles + 1);
			int top = tileY - (radiusTiles + 1);
			int bottom = tileY + (radiusTiles + 1);

			for( int i=left; i<right; i++ ) {
				for( int j=top; j<bottom; j++ ) {
					int distSquared = ( i * i ) + ( j * j );
					if( distSquared > radiusTilesSquared ) { continue; }

					float percentToCenter = 1f - ((float)distSquared / (float)radiusTilesSquared);

					DestructibleTilesProjectile.HitTile( damage, i, j, 1, percentToCenter );
				}
			}
		}
	}
}
