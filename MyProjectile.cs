using System;
using System.Collections.Generic;
using System.Linq;
using HamstarHelpers.Helpers.TileHelpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace DestructibleTiles {
	partial class DestructibleTilesProjectile : GlobalProjectile {
		public static bool HitTile( Projectile projectile, int tileX, int tileY, int totalHits ) {
			HitTile plrTileHits = Main.LocalPlayer.hitTile;

			int tileHitId = plrTileHits.HitObject( tileX, tileY, 1 );
			int dmg = DestructibleTilesProjectile.ComputeHitDamage( Main.tile[tileX, tileY], projectile, totalHits );

			if( plrTileHits.AddDamage(tileHitId, dmg, true) >= 100 ) {
				plrTileHits.Clear( tileHitId );

				WorldGen.KillTile( tileX, tileY, false, false, false );
				if( Main.netMode == 1 ) {
					NetMessage.SendData( MessageID.TileChange, -1, -1, null, 0, (float)tileX, (float)tileY, 0f, 0, 0, 0 );
				}

				Helpers.ParticleHelpers.ParticleFxHelpers.MakeDustCloud( new Vector2((tileX*16) + 8, (tileY*16) + 8), 1, 1f, 1.2f );

				return true;
			}

			return false;
		}



		////////////////
		
		public override bool OnTileCollide( Projectile projectile, Vector2 oldVelocity ) {
			var dim = new Rectangle( (int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height );
			int posX = dim.X + (int)oldVelocity.X;
			int posY = dim.Y + (int)oldVelocity.Y;

			int projRight = posX + dim.Width;
			int projBottom = posY + dim.Height;

			bool respectsPlatforms = false;

			IDictionary<int, int> hits = new Dictionary<int, int>();

			for( int i = (posX >> 4); (i<<4) <= projRight; i++ ) {
				for( int j = (posY >> 4); (j<<4) <= projBottom; j++ ) {
					Tile tile = Main.tile[i, j];
					if( TileHelpers.IsAir(tile) ) { continue; }

					if( TileHelpers.IsSolid(tile, respectsPlatforms, false) ) {
						hits[i] = j;
					}
				}
			}
			
			this.HitTiles( projectile, hits );

			return base.OnTileCollide( projectile, oldVelocity );
		}


		////

		public void HitTiles( Projectile projectile, IDictionary<int, int> hits ) {
			var orderedHits = hits.OrderBy( ( kv ) => {
				var pos = new Vector2( kv.Key, kv.Value );
				
				return Vector2.DistanceSquared( pos, projectile.Center );
			} );

			foreach( var xy in orderedHits ) {
				if( !DestructibleTilesProjectile.HitTile( projectile, xy.Key, xy.Value, hits.Count ) ) {
					break;
				}
			}
		}
	}
}
