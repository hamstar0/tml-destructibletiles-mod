using HamstarHelpers.Components.Config;
using System;
using System.Collections.Generic;

namespace DestructibleTiles {
	public class DestructibleTilesConfigData : ConfigurationDataBase {
		public readonly static string ConfigFileName = "Destructible Tiles Config.json";



		////////////////

		public string VersionSinceUpdate = "";

		public bool DebugModeInfo = false;

		public bool UseVanillaTileDamageScalesUnlessOverridden = true;
		public IDictionary<string, float> TileDamageScale = new Dictionary<string, float>();



		////////////////

		public void SetDefaults() {
			this.TileDamageScale.Clear();
		}


		////////////////

		public bool UpdateToLatestVersion() {
			var mymod = DestructibleTilesMod.Instance;
			var newConfig = new DestructibleTilesConfigData();
			newConfig.SetDefaults();

			var versSince = this.VersionSinceUpdate != "" ?
				new Version( this.VersionSinceUpdate ) :
				new Version();

			if( versSince >= mymod.Version ) {
				return false;
			}

			if( this.VersionSinceUpdate == "" ) {
				this.SetDefaults();
			}

			this.VersionSinceUpdate = mymod.Version.ToString();

			return true;
		}
	}
}
