using Terraria;


namespace DestructibleTiles {
	public static partial class DestructibleTilesAPI {
		public static DestructibleTilesConfigData GetModSettings() {
			return DestructibleTilesMod.Instance.Config;
		}

		public static void SaveModSettingsChanges() {
			DestructibleTilesMod.Instance.ConfigJson.SaveFile();
		}
	}
}
