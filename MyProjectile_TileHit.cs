using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace DestructibleTiles {
	partial class DestructibleTilesProjectile : GlobalProjectile {
		public static bool HitTile( Projectile projectile, int tileX, int tileY, int totalHits, float percent=1f ) {
			var mymod = DestructibleTilesMod.Instance;
			HitTile plrTileHits = Main.LocalPlayer.hitTile;
			
			int tileHitId = plrTileHits.HitObject( tileX, tileY, 1 );
			int dmg = (int)(DestructibleTilesProjectile.ComputeHitDamage( Main.tile[tileX, tileY], projectile, totalHits ) * percent);

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
		
		public void HitTilesInSet( Projectile projectile, IDictionary<int, int> hits ) {
			var mymod = DestructibleTilesMod.Instance;
			/*IOrderedEnumerable<KeyValuePair<int, int>> orderedHits;

			orderedHits = hits.OrderBy( ( kv ) => {
				var pos = new Vector2( kv.Key, kv.Value );
				return Vector2.DistanceSquared( pos, projectile.Center );
			} );*/

			foreach( var xy in hits ) {
				DestructibleTilesProjectile.HitTile( projectile, xy.Key, xy.Value, hits.Count );
			}
		}


		public void HitTilesInRadius( Projectile projectile, int addedRadiusInWorldUnits ) {
			int radius = ((projectile.width + projectile.height) / 4) + addedRadiusInWorldUnits;
			int radiusTiles = (int)Math.Round( (double)(radius / 16) );
			int radiusTilesSquared = radiusTiles * radiusTiles;

			int tileX = (int)projectile.Center.X >> 4;
			int tileY = (int)projectile.Center.Y >> 4;

			int left = tileX - radiusTiles;
			int right = tileX + radiusTiles;
			int top = tileY - radiusTiles;
			int bottom = tileY + radiusTiles;

			for( int i=left; i<right; i++ ) {
				for( int j=top; j<bottom; j++ ) {
					int distSquared = ( i * i ) + ( j * j );
					if( distSquared > radiusTilesSquared ) { continue; }

					float percentToCenter = 1f - ((float)distSquared / (float)radiusTilesSquared);

					DestructibleTilesProjectile.HitTile( projectile, i, j, 1, percentToCenter );
				}
			}
		}
	}
}
