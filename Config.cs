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

		public float AllDamagesScale = 1f;

		public IDictionary<string, int>		ProjectileTileDamageDefaults = new Dictionary<string, int>();
		public IDictionary<string, int>		ProjectileTileDamageOverrides = new Dictionary<string, int>();
		public IDictionary<string, int>		ProjectilesAsExplosivesAndRadius = new Dictionary<string, int>();
		public IDictionary<string, int>		ProjectilesAsConsecutiveHittingAndCooldown = new Dictionary<string, int>();
		//public IDictionary<string, float>	ProjectilesAsPhysicsObjectsAndMaxVelocity = new Dictionary<string, float>();

		public IDictionary<string, float>	TileDamageScaleOverrides = new Dictionary<string, float>();
		public IDictionary<string, float>	TileArmor = new Dictionary<string, float>();


		////

		public string _OLD_SETTINGS_BELOW_ = "";

		public IDictionary<string, int[]> ProjectilesAsExplosivesAndRadiusAndDamage = new Dictionary<string, int[]>();



		////////////////

		public DestructibleTilesConfigData() {
		}

		public void SetDefaults() {
			this.ProjectileTileDamageDefaults.Clear();
			this.ProjectileTileDamageOverrides.Clear();
			this.ProjectilesAsExplosivesAndRadius.Clear();
			//this.ProjectilesAsConsecutiveHittingAndCooldown.Clear();
			//this.ProjectilesAsPhysicsObjectsAndMaxVelocity.Clear();
			this.TileDamageScaleOverrides.Clear();
			this.TileArmor.Clear();

			this.TileDamageScaleOverrides["Terraria." + TileID.MartianConduitPlating] = 0.1f;

			this.ProjectileTileDamageDefaults["Terraria.30"] = 60;		//Grenade
			this.ProjectileTileDamageDefaults["Terraria.108"] = 500;    //Explosives
			this.ProjectileTileDamageDefaults["Terraria.133"] = 40;     //Grenade I
			this.ProjectileTileDamageDefaults["Terraria.134"] = 40;     //Rocket I
			this.ProjectileTileDamageDefaults["Terraria.135"] = 40;     //Proximity Mine I
			this.ProjectileTileDamageDefaults["Terraria.136"] = 40;     //Grenade II
			this.ProjectileTileDamageDefaults["Terraria.137"] = 40;     //Rocket II
			this.ProjectileTileDamageDefaults["Terraria.138"] = 40;     //Proximity Mine II
			this.ProjectileTileDamageDefaults["Terraria.139"] = 65;     //Grenade III
			this.ProjectileTileDamageDefaults["Terraria.140"] = 65;     //Rocket III
			this.ProjectileTileDamageDefaults["Terraria.141"] = 65;     //Proximity Mine III
			this.ProjectileTileDamageDefaults["Terraria.142"] = 65;     //Grenade IV
			this.ProjectileTileDamageDefaults["Terraria.143"] = 65;     //Rocket IV
			this.ProjectileTileDamageDefaults["Terraria.164"] = 250;    //Land Mine
			this.ProjectileTileDamageDefaults["Terraria.338"] = 40;     //Rocket (I; snowman)
			this.ProjectileTileDamageDefaults["Terraria.339"] = 40;     //Rocket (II; snowman)
			this.ProjectileTileDamageDefaults["Terraria.340"] = 40;     //Rocket (III; snowman)
			this.ProjectileTileDamageDefaults["Terraria.341"] = 40;     //Rocket (IV; snowman)
			this.ProjectileTileDamageDefaults["Terraria.397"] = 60;     //Sticky Grenade
			this.ProjectileTileDamageDefaults["Terraria.517"] = 65;     //Bouncy Grenade
			this.ProjectileTileDamageDefaults["Terraria.588"] = 30;		//Happy Grenade

			this.ProjectilesAsConsecutiveHittingAndCooldown["Terraria."+ProjectileID.MolotovFire] = 60 * 2;
			this.ProjectilesAsConsecutiveHittingAndCooldown["Terraria."+ProjectileID.MolotovFire2] = 60 * 2;
			this.ProjectilesAsConsecutiveHittingAndCooldown["Terraria."+ProjectileID.MolotovFire3] = 60 * 2;
		}

		public void SetProjectileDefaults() {
			if( !this.AutoLoadDefaultExplosiveProjectiles ) { return; }

			IDictionary<string, Tuple<int, int>> explosiveProjs = DestructibleTilesProjectile.GetExplosives();

			foreach( var kv in explosiveProjs ) {
				string projName = kv.Key;
				int radius = kv.Value.Item1;
				int damage = kv.Value.Item2;

				if( !this.ProjectilesAsExplosivesAndRadius.ContainsKey(projName) ) {
					this.ProjectilesAsExplosivesAndRadius[projName] = radius;
				}
				if( !this.ProjectileTileDamageDefaults.ContainsKey( projName ) ) {
					this.ProjectileTileDamageDefaults[projName] = damage;
				}
			}
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
