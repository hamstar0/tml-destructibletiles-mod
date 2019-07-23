using System;
using Terraria;
using Terraria.ModLoader;


namespace DestructibleTiles {
	class DestructibleTilesPlayer : ModPlayer {
		public override bool CloneNewInstances => false;



		////////////////

		public override void SyncPlayer( int toWho, int fromWho, bool newPlayer ) {
			if( Main.netMode == 2 ) {
				if( toWho == -1 && fromWho == this.player.whoAmI ) {
					this.OnConnectServer();
				}
			}
		}

		public override void OnEnterWorld( Player player ) {
			if( player.whoAmI != Main.myPlayer ) { return; }
			if( this.player.whoAmI != Main.myPlayer ) { return; }

			var mymod = (DestructibleTilesMod)this.mod;

			if( Main.netMode == 0 ) {
				this.OnConnectSingle();
			} else if( Main.netMode == 1 ) {
				this.OnConnectClient();
			}
		}


		////////////////

		private void OnConnectSingle() {
			var mymod = (DestructibleTilesMod)this.mod;

			mymod.Config.SetProjectileDefaults();
		}

		private void OnConnectClient() {
		}

		private void OnConnectServer() {
		}



		////////////////

		/*public override void PreUpdate() {
			Player plr = this.player;
			if( plr.whoAmI != Main.myPlayer ) { return; }
			if( plr.dead ) { return; }
			
			...
		}*/
	}
}
