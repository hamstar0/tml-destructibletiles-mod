using DestructibleTiles.MultiHitTile;
using HamstarHelpers.Helpers.TModLoader.Mods;
using Terraria.ModLoader;


namespace DestructibleTiles {
	partial class DestructibleTilesMod : Mod {
		public static DestructibleTilesMod Instance { get; private set; }



		////////////////

		public DestructibleTilesConfig Config { get; private set; }

		public TileDataManager TileDataMngr;



		////////////////

		public DestructibleTilesMod() {
			this.Config = new DestructibleTilesConfig();
		}

		////////////////

		public override void Load() {
			DestructibleTilesMod.Instance = this;

			this.TileDataMngr = new TileDataManager();
		}

		public override void Unload() {
			DestructibleTilesMod.Instance = null;
		}

		////

		public override void PostSetupContent() {
			this.Config.SetProjectileDefaults();
		}


		////////////////

		public override object Call( params object[] args ) {
			return ModBoilerplateHelpers.HandleModCall( typeof( DestructibleTilesAPI ), args );
		}
	}
}
