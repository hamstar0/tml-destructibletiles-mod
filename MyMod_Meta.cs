using HamstarHelpers.Components.Config;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;


namespace DestructibleTiles {
	partial class DestructibleTilesMod : Mod {
		public static string GithubUserName => "hamstar0";
		public static string GithubProjectName => "tml-destructibletiles-mod";

		public static string ConfigFileRelativePath {
			get { return ConfigurationDataBase.RelativePath + Path.DirectorySeparatorChar + DestructibleTilesConfigData.ConfigFileName; }
		}
		public static void ReloadConfigFromFile() {
			if( Main.netMode != 0 ) {
				throw new Exception( "Cannot reload configs outside of single player." );
			}
			if( !DestructibleTilesMod.Instance.ConfigJson.LoadFile() ) {
				DestructibleTilesMod.Instance.ConfigJson.SaveFile();
			}
		}

		public static void ResetConfigFromDefaults() {
			if( Main.netMode != 0 ) {
				throw new Exception( "Cannot reset to default configs outside of single player." );
			}

			var configData = new DestructibleTilesConfigData();
			configData.SetDefaults();

			DestructibleTilesMod.Instance.ConfigJson.SetData( configData );
			DestructibleTilesMod.Instance.ConfigJson.SaveFile();
		}
	}
}
