using HamstarHelpers.Components.DataStructures;
using HamstarHelpers.Helpers.TmlHelpers;
using HamstarHelpers.Services.Timers;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Utilities;


namespace DestructibleTiles.MultiHitTile {
	public partial class TileDataManager {
		private static object MyLock = new object();

		private static UnifiedRandom Rand = new UnifiedRandom();
		private static int LastCrack = -1;



		////////////////

		public static bool IsValidTile( int x, int y ) {
			if( !WorldGen.InWorld( x, y, 0 ) ) { return false; }
			Tile tile = Main.tile[x, y];

			if( HamstarHelpers.Helpers.TileHelpers.TileHelpers.IsAir( tile ) ) { return false; }
			if( !Main.tileSolid[(int)tile.type] ) { return false; }
			if( Main.tileSolidTop[(int)tile.type] ) { return false; }

			return true;
		}

		////////////////

		public static void RerollCrackStyle( TileData data ) {
			do {
				data.CrackStyle = TileDataManager.Rand.Next( 4 );
			} while( data.CrackStyle == TileDataManager.LastCrack );

			TileDataManager.LastCrack = data.CrackStyle;
		}



		////////////////

		public IDictionary<int, IDictionary<int, TileData>> Data = new Dictionary<int, IDictionary<int, TileData>>();

		private Func<bool> OnTickGet;



		////////////////

		public TileDataManager() {
			this.OnTickGet = Timers.MainOnTickGet();
			Main.OnTick += TileDataManager._Update;

			if( !Main.dedServ ) {
				Overlays.Scene["TileEffects"] = new TileEffectsOverlay();
				Overlays.Scene.Activate( "TileEffects" );

				Main.OnPostDraw += TileDataManager._DrawPostDrawAll;
			}
		}

		////

		~TileDataManager() {
			if( !Main.dedServ ) {
				Main.OnPostDraw -= TileDataManager._DrawPostDrawAll;
			}
			Main.OnTick -= TileDataManager._Update;
		}


		////////////////

		public int AddDamage( int x, int y, int damage ) {
			if( damage == 0 ) {
				return 0;
			}

			TileData data;

			lock( TileDataManager.MyLock ) {
				data = this.Data.Get2DOrDefault( x, y );
				if( data == null ) {
					data = new TileData();
					this.Data.Set2D( x, y, data );
				}
			}

			data.Damage += damage;
			data.TTL = 60;
			data.AnimationTimeElapsed = 0;
			//data.AnimationDirection = ( Main.rand.NextFloat() * 6.28318548f ).ToRotationVector2() * 2f;

			return data.Damage;
		}
	}
}
