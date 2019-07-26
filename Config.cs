using HamstarHelpers.Helpers.TModLoader.Configs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using Terraria.ModLoader.Config;


namespace DestructibleTiles {
	public class ProjectileStateDefinition {
		[Label("Hurts players or friendly NPCs")]
		public int IsHostile;
		[Label("Hurts enemies")]
		public int IsFriendly;
		[Label("NPC-made projectile")]
		public int IsNPC;
		[Label("Damage amount")]
		public int Amount;

		////

		[JsonIgnore]
		internal bool? IsHostileFlag => this.IsHostile == 0 ? (bool?)null :
			this.IsHostile == 1 ? true : false;
		[JsonIgnore]
		internal bool? IsFriendlyFlag => this.IsFriendly == 0 ? (bool?)null :
			this.IsFriendly == 1 ? true : false;
		[JsonIgnore]
		internal bool? IsNPCFlag => this.IsNPC == 0 ? (bool?)null :
			this.IsNPC == 1 ? true : false;



		////////////////

		public ProjectileStateDefinition( int isHostile, int isFriendly, int isPlayer, int amount ) {
			this.IsHostile = isHostile;
			this.IsFriendly = isFriendly;
			this.IsNPC = isPlayer;
			this.Amount = amount;
		}
	}




	public class DestructibleTilesConfig : ModConfig {
		[JsonIgnore]
		private bool ApplyDefaults = false;


		////

		[Label( "Debug Mode" )]
		[Tooltip( "Logs or displays developer-relevant information." )]
		public bool DebugModeInfo = false;


		[Label( "Auto-load explosive projectils into config" )]
		[Tooltip( "Auto-adds all explosive projectiles into the below projectile lists." )]
		[DefaultValue( true )]
		public bool AutoLoadDefaultExplosiveProjectiles = true;


		[Label( "Destroyed tiles drop block items" )]
		public bool DestroyedTilesDropItems = false;


		[Label( "Use vanilla tile damage scaling (unless overridden)" )]
		[DefaultValue( true )]
		public bool UseVanillaTileDamageScalesUnlessOverridden = true;


		[Label( "Damage multiplier for all projectiles" )]
		[Range( 0f, Single.MaxValue )]
		[DefaultValue(1f)]
		public float AllDamagesScale = 1f;


		[Label( "Projectile damage to tiles (accepts scaling)" )]
		public IDictionary<string, ProjectileStateDefinition>	ProjectileDamageDefaults
			= new Dictionary<string, ProjectileStateDefinition>();

		[Label( "Projectile damage to tiles (overrides all)" )]
		public IDictionary<string, ProjectileStateDefinition>	ProjectileDamageOverrides
			= new Dictionary<string, ProjectileStateDefinition>();

		[Label( "Explosive projectives with their radiuses" )]
		public IDictionary<string, ProjectileStateDefinition>	ProjectilesAsExplosivesAndRadius
			= new Dictionary<string, ProjectileStateDefinition>();

		[Label( "Consecutive-hitting projectiles with their cooldowns" )]
		public IDictionary<string, ProjectileStateDefinition>	ProjectilesAsConsecutiveHittingAndCooldown
			= new Dictionary<string, ProjectileStateDefinition>();
		//public IDictionary<string, float>	ProjectilesAsPhysicsObjectsAndMaxVelocity = new Dictionary<string, float>();


		[Label( "Tile damage multiplier" )]
		public IDictionary<string, PositiveSingleDefinition>	TileDamageScaleOverrides
			= new Dictionary<string, PositiveSingleDefinition>();

		[Label( "Tile armor" )]
		public IDictionary<string, PositiveIntDefinition>		TileArmor
			= new Dictionary<string, PositiveIntDefinition>();


		[Label( "Beam damage scale" )]
		[Range( 0f, Single.MaxValue )]
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
			this.ApplyDefaults = this.ProjectileDamageDefaults == null;

			this.ProjectileDamageDefaults = this.ProjectileDamageDefaults
				?? new Dictionary<string, ProjectileStateDefinition>();
			this.ProjectileDamageOverrides = this.ProjectileDamageOverrides
				?? new Dictionary<string, ProjectileStateDefinition>();
			this.ProjectilesAsExplosivesAndRadius = this.ProjectilesAsExplosivesAndRadius
				?? new Dictionary<string, ProjectileStateDefinition>();
			this.ProjectilesAsConsecutiveHittingAndCooldown = this.ProjectilesAsConsecutiveHittingAndCooldown
				?? new Dictionary<string, ProjectileStateDefinition>();
			this.TileDamageScaleOverrides = this.TileDamageScaleOverrides
				?? new Dictionary<string, PositiveSingleDefinition>();
			this.TileArmor = this.TileArmor
				?? new Dictionary<string, PositiveIntDefinition>();

			if( this.ApplyDefaults ) {
				this.TileDamageScaleOverrides["Terraria MartianConduitPlating"] = new PositiveSingleDefinition( 0.1f );

				this.ProjectileDamageDefaults["Terraria Grenade"] = new ProjectileStateDefinition( 0, 0, 0, 60 );
				this.ProjectileDamageDefaults["Terraria Explosives"] = new ProjectileStateDefinition( 0, 0, 0, 500 );
				this.ProjectileDamageDefaults["Terraria GrenadeI"] = new ProjectileStateDefinition( 0, 0, 0, 40 );
				this.ProjectileDamageDefaults["Terraria RocketI"] = new ProjectileStateDefinition( 0, 0, 0, 40 );
				this.ProjectileDamageDefaults["Terraria ProximityMineI"] = new ProjectileStateDefinition( 0, 0, 0, 40 );
				this.ProjectileDamageDefaults["Terraria GrenadeII"] = new ProjectileStateDefinition( 0, 0, 0, 40 );
				this.ProjectileDamageDefaults["Terraria RocketII"] = new ProjectileStateDefinition( 0, 0, 0, 40 );
				this.ProjectileDamageDefaults["Terraria ProximityMineII"] = new ProjectileStateDefinition( 0, 0, 0, 40 );
				this.ProjectileDamageDefaults["Terraria GrenadeIII"] = new ProjectileStateDefinition( 0, 0, 0, 65 );
				this.ProjectileDamageDefaults["Terraria RocketIII"] = new ProjectileStateDefinition( 0, 0, 0, 65 );
				this.ProjectileDamageDefaults["Terraria ProximityMineIII"] = new ProjectileStateDefinition( 0, 0, 0, 65 );
				this.ProjectileDamageDefaults["Terraria GrenadeIV"] = new ProjectileStateDefinition( 0, 0, 0, 65 );
				this.ProjectileDamageDefaults["Terraria RocketIV"] = new ProjectileStateDefinition( 0, 0, 0, 65 );
				this.ProjectileDamageDefaults["Terraria ProximityMineIV"] = new ProjectileStateDefinition( 0, 0, 0, 65 );
				this.ProjectileDamageDefaults["Terraria Landmine"] = new ProjectileStateDefinition( 0, 0, 0, 250 );
				this.ProjectileDamageDefaults["Terraria RocketSnowmanI"] = new ProjectileStateDefinition( 0, 0, 0, 40 );
				this.ProjectileDamageDefaults["Terraria RocketSnowmanII"] = new ProjectileStateDefinition( 0, 0, 0, 40 );
				this.ProjectileDamageDefaults["Terraria RocketSnowmanIII"] = new ProjectileStateDefinition( 0, 0, 0, 40 );
				this.ProjectileDamageDefaults["Terraria RocketSnowmanIV"] = new ProjectileStateDefinition( 0, 0, 0, 40 );
				this.ProjectileDamageDefaults["Terraria StickyGrenade"] = new ProjectileStateDefinition( 0, 0, 0, 60 );
				this.ProjectileDamageDefaults["Terraria BouncyGrenade"] = new ProjectileStateDefinition( 0, 0, 0, 65 );
				this.ProjectileDamageDefaults["Terraria PartyGirlGrenade"] = new ProjectileStateDefinition( 0, 0, 0, 30 );

				this.ProjectilesAsConsecutiveHittingAndCooldown["Terraria MolotovFire"] = new ProjectileStateDefinition( 0, 0, 0, 45 );
				this.ProjectilesAsConsecutiveHittingAndCooldown["Terraria MolotovFire2"] = new ProjectileStateDefinition( 0, 0, 0, 45 );
				this.ProjectilesAsConsecutiveHittingAndCooldown["Terraria MolotovFire3"] = new ProjectileStateDefinition( 0, 0, 0, 45 );

				this.TileArmor["Terraria LihzahrdBrick"] = new PositiveIntDefinition( 150 );
			}
		}

		public void SetProjectileDefaults() {
			if( !this.AutoLoadDefaultExplosiveProjectiles || !this.ApplyDefaults ) {
				return;
			}
			this.ApplyDefaults = false;

			IDictionary<string, Tuple<int, int>> explosiveProjs = DestructibleTilesProjectile.GetExplosives();

			foreach( var kv in explosiveProjs ) {
				string projName = kv.Key;
				int radius = kv.Value.Item1;
				int damage = kv.Value.Item2;

				if( !this.ProjectilesAsExplosivesAndRadius.ContainsKey( projName ) ) {
					this.ProjectilesAsExplosivesAndRadius[projName] = new ProjectileStateDefinition( 0, 0, 0, radius );
				}
				if( !this.ProjectileDamageDefaults.ContainsKey( projName ) ) {
					this.ProjectileDamageDefaults[projName] = new ProjectileStateDefinition( 0, 0, 0, damage );
				}
			}
		}
	}
}
