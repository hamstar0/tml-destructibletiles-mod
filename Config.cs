using HamstarHelpers.Components.Config;
using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Collections.Generic;
using Terraria.ID;


namespace DestructibleTiles {
	public class DestructibleTilesConfigData : ConfigurationDataBase {
		public readonly static string ConfigFileName = "Destructible Tiles Config.json";



		////////////////

		public string VersionSinceUpdate = "";

		public bool DebugModeInfo = false;

		public bool AutoLoadDefaultExplosiveProjectiles = true;

		public bool DestroyedTilesDropItems = false;
		
		public bool UseVanillaTileDamageScalesUnlessOverridden = true;

		public IDictionary<string, int[]>	ProjectilesAsExplosivesAndRadiusAndDamage = new Dictionary<string, int[]>();
		public IDictionary<string, int[]>	ProjectilesAsConsecutiveHittingAndCooldown = new Dictionary<string, int[]>();
		public IDictionary<string, float>	ProjectilesAsPhysicsObjectsAndMaxVelocity = new Dictionary<string, float>();
		public IDictionary<string, int>		ProjectilesAsConsecutiveHittersAndCooldowns = new Dictionary<string, int>();

		public IDictionary<string, float>	TileDamageScaleOverrides = new Dictionary<string, float>();
		public IDictionary<string, float>	TileArmor = new Dictionary<string, float>();



		////////////////

		public DestructibleTilesConfigData() {
		}

		public void SetDefaults() {
			this.ProjectilesAsExplosivesAndRadiusAndDamage.Clear();
			this.ProjectilesAsConsecutiveHittingAndCooldown.Clear();
			this.ProjectilesAsPhysicsObjectsAndMaxVelocity.Clear();
			this.ProjectilesAsConsecutiveHittersAndCooldowns.Clear();
			this.TileDamageScaleOverrides.Clear();
			this.TileArmor.Clear();

			this.TileDamageScaleOverrides["Terraria." + TileID.MartianConduitPlating] = 0.1f;
		}

		public void SetProjectileDefaults() {
			if( !this.AutoLoadDefaultExplosiveProjectiles ) { return; }
			this.ProjectilesAsExplosivesAndRadiusAndDamage = DestructibleTilesProjectile.GetExplosives();
		}

		public override void OnLoad( bool success ) {
			if( !success ) {
				this.SetDefaults();
			}
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
