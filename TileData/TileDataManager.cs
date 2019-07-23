using HamstarHelpers.Components.DataStructures;
using HamstarHelpers.Helpers.DotNET.Extensions;
using HamstarHelpers.Helpers.Tiles;
using HamstarHelpers.Services.Timers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Utilities;


namespace DestructibleTiles.MultiHitTile {
	public partial class TileDataManager {
		private const int HitAnimationMaxDuration = 20;

		////

		private static UnifiedRandom Rand = new UnifiedRandom();
		private static int LastCrack = -1;



		////////////////

		public static bool IsValidTile( int tileX, int tileY ) {
			if( !WorldGen.InWorld( tileX, tileY, 0 ) ) { return false; }
			Tile tile = Main.tile[tileX, tileY];

			if( TileHelpers.IsAir(tile) ) { return false; }
			if( !tile.active() ) { return false; }
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

		public IDictionary<int, IDictionary<int, TileData>> Data = new ConcurrentDictionary<int, IDictionary<int, TileData>>();

		private Func<bool> OnTickGet;



		////////////////

		public TileDataManager() {
			this.OnTickGet = Timers.MainOnTickGet();
			Main.OnTick += TileDataManager._Update;

			if( !Main.dedServ ) {
				Overlays.Scene["TileDamageEffects"] = new TileEffectsOverlay();
				Overlays.Scene.Activate( "TileDamageEffects" );
			}
		}

		////

		~TileDataManager() {
			Main.OnTick -= TileDataManager._Update;
			Overlays.Scene["TileDamageEffects"].Deactivate();
		}


		////////////////

		public int AddDamage( int x, int y, int damage ) {
			if( damage == 0 ) {
				return -1;
			}

			bool isValid = TileDataManager.IsValidTile( x, y );
			TileData data;

			data = this.Data.Get2DOrDefault( x, y );
			if( data == null ) {
				if( !isValid ) {
					return -1;
				}

				data = new TileData();
				this.Data.Set2D( x, y, data );
			} else {
				if( !isValid ) {
					this.Data[ x ].Remove( y );
					return -1;
				}
			}

			data.Damage += damage;
			data.TTL = 60 * 60;
			data.AnimationTimeDuration = TileDataManager.HitAnimationMaxDuration;
			//data.AnimationDirection = ( Main.rand.NextFloat() * 6.28318548f ).ToRotationVector2() * 2f;

			return data.Damage;
		}
	}
}
