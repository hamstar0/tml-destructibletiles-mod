using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using Terraria.ModLoader.Config;


namespace DestructibleTiles {
	public class DestructibleTilesConfig : ModConfig {
		[Label("Debug Mode")]
		[Tooltip("Logs or displays developer-relevant information.")]
		public bool DebugModeInfo = false;

		[Label("Auto-load explosive projectils into config")]
		[Tooltip("Auto-adds all explosive projectiles into the below projectile lists.")]
		[DefaultValue(true)]
		public bool AutoLoadDefaultExplosiveProjectiles = true;

		[Label("Destroyed tiles drop block items")]
		public bool DestroyedTilesDropItems = false;
		
		[Label("Use vanilla tile damage scaling (unless overridden)")]
		[DefaultValue(true)]
		public bool UseVanillaTileDamageScalesUnlessOverridden = true;

		[Label("Damage multiplier for all projectiles")]
		[DefaultValue(1f)]
		public float AllDamagesScale = 1f;

		[Label( "Projectile damage to tiles (accepts scaling)" )]
		public IDictionary<string, int>		ProjectileDamageDefaults = new Dictionary<string, int>();
		[Label( "Projectile damage to tiles (overrides all)" )]
		public IDictionary<string, int>		ProjectileDamageOverrides = new Dictionary<string, int>();
		[Label( "Explosive projectives with their radiuses" )]
		public IDictionary<string, int>		ProjectilesAsExplosivesAndRadius = new Dictionary<string, int>();
		[Label( "Consecutive-hitting projectiles with their cooldowns" )]
		public IDictionary<string, int>		ProjectilesAsConsecutiveHittingAndCooldown = new Dictionary<string, int>();
		//public IDictionary<string, float>	ProjectilesAsPhysicsObjectsAndMaxVelocity = new Dictionary<string, float>();

		[Label( "Tile damage multiplier" )]
		public IDictionary<string, float>	TileDamageScaleOverrides = new Dictionary<string, float>();
		[Label( "Tile armor" )]
		public IDictionary<string, float>	TileArmor = new Dictionary<string, float>();

		[Label( "Beam damage scale" )]
		[DefaultValue( 1f / 30f )]
		public float BeamDamageScale = 1f / 30f;



		////////////////

		public override ConfigScope Mode => ConfigScope.ServerSide;



		////////////////

		public override ModConfig Clone() {
			var clone = (DestructibleTilesConfig)base.Clone();

			clone.ProjectileDamageDefaults = this.ProjectileDamageDefaults?.ToDictionary( kv=>kv.Key, kv=>kv.Value );
			clone.ProjectileDamageOverrides = this.ProjectileDamageOverrides?.ToDictionary( kv => kv.Key, kv => kv.Value );
			clone.ProjectilesAsExplosivesAndRadius = this.ProjectilesAsExplosivesAndRadius?.ToDictionary( kv => kv.Key, kv => kv.Value );
			clone.ProjectilesAsConsecutiveHittingAndCooldown = this.ProjectilesAsConsecutiveHittingAndCooldown?.ToDictionary( kv => kv.Key, kv => kv.Value );
			clone.TileDamageScaleOverrides = this.TileDamageScaleOverrides?.ToDictionary( kv => kv.Key, kv => kv.Value );
			clone.TileArmor = this.TileArmor?.ToDictionary( kv => kv.Key, kv => kv.Value );

			return clone;
		}

		[OnDeserialized]
		internal void OnDeserializedMethod( StreamingContext context ) {
			this.ProjectileDamageDefaults = this.ProjectileDamageDefaults
				?? new Dictionary<string, int>();
			this.ProjectileDamageOverrides = this.ProjectileDamageOverrides
				?? new Dictionary<string, int>();
			this.ProjectilesAsExplosivesAndRadius = this.ProjectilesAsExplosivesAndRadius
				?? new Dictionary<string, int>();
			this.ProjectilesAsConsecutiveHittingAndCooldown = this.ProjectilesAsConsecutiveHittingAndCooldown
				?? new Dictionary<string, int>();
			this.TileDamageScaleOverrides = this.TileDamageScaleOverrides
				?? new Dictionary<string, float>();
			this.TileArmor = this.TileArmor
				?? new Dictionary<string, float>();

			this.TileDamageScaleOverrides["Terraria MartianConduitPlating"] = 0.1f;

			this.ProjectileDamageDefaults["Terraria Grenade"] = 60;		//Grenade
			this.ProjectileDamageDefaults["Terraria Explosives"] = 500;    //Explosives
			this.ProjectileDamageDefaults["Terraria GrenadeI"] = 40;     //Grenade I
			this.ProjectileDamageDefaults["Terraria RocketI"] = 40;     //Rocket I
			this.ProjectileDamageDefaults["Terraria ProximityMineI"] = 40;     //Proximity Mine I
			this.ProjectileDamageDefaults["Terraria GrenadeII"] = 40;     //Grenade II
			this.ProjectileDamageDefaults["Terraria RocketII"] = 40;     //Rocket II
			this.ProjectileDamageDefaults["Terraria ProximityMineII"] = 40;     //Proximity Mine II
			this.ProjectileDamageDefaults["Terraria GrenadeIII"] = 65;     //Grenade III
			this.ProjectileDamageDefaults["Terraria RocketIII"] = 65;     //Rocket III
			this.ProjectileDamageDefaults["Terraria ProximityMineIII"] = 65;     //Proximity Mine III
			this.ProjectileDamageDefaults["Terraria GrenadeIV"] = 65;     //Grenade IV
			this.ProjectileDamageDefaults["Terraria RocketIV"] = 65;     //Rocket IV
			this.ProjectileDamageDefaults["Terraria ProximityMineIV"] = 65;     //Rocket IV
			this.ProjectileDamageDefaults["Terraria Landmine"] = 250;    //Land Mine
			this.ProjectileDamageDefaults["Terraria RocketSnowmanI"] = 40;     //Rocket (I; snowman)
			this.ProjectileDamageDefaults["Terraria RocketSnowmanII"] = 40;     //Rocket (II; snowman)
			this.ProjectileDamageDefaults["Terraria RocketSnowmanIII"] = 40;     //Rocket (III; snowman)
			this.ProjectileDamageDefaults["Terraria RocketSnowmanIV"] = 40;     //Rocket (IV; snowman)
			this.ProjectileDamageDefaults["Terraria StickyGrenade"] = 60;     //Sticky Grenade
			this.ProjectileDamageDefaults["Terraria BouncyGrenade"] = 65;     //Bouncy Grenade
			this.ProjectileDamageDefaults["Terraria PartyGirlGrenade"] = 30;		//Happy Grenade

			this.ProjectilesAsConsecutiveHittingAndCooldown["Terraria MolotovFire"] = 45;
			this.ProjectilesAsConsecutiveHittingAndCooldown["Terraria MolotovFire2"] = 45;
			this.ProjectilesAsConsecutiveHittingAndCooldown["Terraria MolotovFire3"] = 45;

			this.TileArmor["Terraria LihzahrdBrick"] = 150;
		}

		public void SetProjectileDefaults() {
			if( !this.AutoLoadDefaultExplosiveProjectiles ) {
				return;
			}
			this.AutoLoadDefaultExplosiveProjectiles = false;

			IDictionary<string, Tuple<int, int>> explosiveProjs = DestructibleTilesProjectile.GetExplosives();

			foreach( var kv in explosiveProjs ) {
				string projName = kv.Key;
				int radius = kv.Value.Item1;
				int damage = kv.Value.Item2;

				if( !this.ProjectilesAsExplosivesAndRadius.ContainsKey( projName ) ) {
					this.ProjectilesAsExplosivesAndRadius[projName] = radius;
				}
				if( !this.ProjectileDamageDefaults.ContainsKey( projName ) ) {
					this.ProjectileDamageDefaults[projName] = damage;
				}
			}
		}
	}
}
