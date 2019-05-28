using DestructibleTiles.MultiHitTile;
using HamstarHelpers.Components.Config;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.TmlHelpers.ModHelpers;
using Terraria.ModLoader;


namespace DestructibleTiles {
	partial class DestructibleTilesMod : Mod {
		public static DestructibleTilesMod Instance { get; private set; }



		////////////////

		public JsonConfig<DestructibleTilesConfigData> ConfigJson { get; private set; }
		public DestructibleTilesConfigData Config => this.ConfigJson.Data;

		public TileDataManager TileDataMngr;



		////////////////

		public DestructibleTilesMod() {
			this.ConfigJson = new JsonConfig<DestructibleTilesConfigData>(
				DestructibleTilesConfigData.ConfigFileName,
				ConfigurationDataBase.RelativePath,
				new DestructibleTilesConfigData()
			);
		}

		////////////////

		public override void Load() {
			DestructibleTilesMod.Instance = this;

			this.LoadConfig();

			this.TileDataMngr = new TileDataManager();
		}

		private void LoadConfig() {
			if( !this.ConfigJson.LoadFile() ) {
				this.Config.SetDefaults();
				this.ConfigJson.SaveFile();
				ErrorLogger.Log( "Destructible Tiles config " + this.Version.ToString() + " created." );
			}

			if( this.Config.UpdateToLatestVersion() ) {
				ErrorLogger.Log( "Destructible Tiles updated to " + this.Version.ToString() );
				this.ConfigJson.SaveFile();
			}
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
