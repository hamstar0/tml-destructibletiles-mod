using System;
using System.Collections.Generic;
using System.Linq;
using HamstarHelpers.Helpers.TileHelpers;
using HamstarHelpers.Services.Timers;
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
		
		public void HitTiles( Projectile projectile, IDictionary<int, int> hits ) {
			var mymod = DestructibleTilesMod.Instance;
			IOrderedEnumerable<KeyValuePair<int, int>> orderedHits;

			orderedHits = hits.OrderBy( ( kv ) => {
				var pos = new Vector2( kv.Key, kv.Value );
				return Vector2.DistanceSquared( pos, projectile.Center );
			} );

			bool isFirst = true;
			foreach( var xy in orderedHits ) {
				DestructibleTilesProjectile.HitTile( projectile, xy.Key, xy.Value, hits.Count );

				if( isFirst ) {
					isFirst = false;
					projectile.damage = (int)((float)projectile.damage * mymod.Config.AfterFirstTileHitDamageScale);
				}
			}
		}
	}
}
