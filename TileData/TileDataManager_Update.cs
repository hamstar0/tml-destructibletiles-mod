using HamstarHelpers.Helpers.TmlHelpers;
using System;
using System.Linq;


namespace DestructibleTiles.MultiHitTile {
	public partial class TileDataManager {
		private static void _Update() { // <- Just in case references are doing something funky...
			var mymod = DestructibleTilesMod.Instance;
			if( mymod == null || mymod.TileDataMngr == null ) { return; }

			if( mymod.TileDataMngr.OnTickGet() ) {
				mymod.TileDataMngr.Update();
			}
		}


		////////////////

		internal void Update() {
			if( !LoadHelpers.IsWorldBeingPlayed() ) { return; }

			lock( TileDataManager.MyLock ) {
				foreach( var kv in this.Data ) {
					foreach( var kv2 in kv.Value.ToArray() ) {
						int x = kv.Key;
						int y = kv2.Key;
						TileData data = kv2.Value;

						if( !TileDataManager.IsValidTile(x, y) ) {
							kv.Value.Remove( y );
						} else if( data.Damage > 0 && data.TTL-- <= 0 ) {
							data.Damage = 0;
						}
					}
				}
			}
		}
	}
}
