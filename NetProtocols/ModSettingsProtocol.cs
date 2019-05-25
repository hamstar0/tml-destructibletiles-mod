using HamstarHelpers.Components.Network;


namespace DestructibleTiles.NetProtocols {
	class ModSettingsProtocol : PacketProtocolRequestToServer {
		public DestructibleTilesConfigData ModSettings;



		////////////////

		private ModSettingsProtocol() { }


		////////////////

		protected override void InitializeServerSendData( int who ) {
			this.ModSettings = DestructibleTilesMod.Instance.Config;
		}

		////////////////
		
		protected override void ReceiveReply() {
			DestructibleTilesMod.Instance.ConfigJson.SetData( this.ModSettings );
		}
	}
}
