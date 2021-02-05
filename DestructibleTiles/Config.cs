using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Terraria.ID;
using Terraria.ModLoader.Config;
using HamstarHelpers.Helpers.TModLoader.Configs;
using HamstarHelpers.Classes.UI.ModConfig;


namespace DestructibleTiles {
	class MyFloatInputElement : FloatInputElement { }





	public class ProjectileStateDefinition {
		[Label("Hurts players or friendly NPCs")]
		public int IsHostile;
		[Label("Hurts enemies")]
		public int IsFriendly;
		[Label("NPC-made projectile")]
		public int IsNPC;
		[Label("Damage amount")]
		public int Amount;



		////////////////

		internal bool? IsHostileFlag => this.IsHostile == 0 ? (bool?)null : this.IsHostile == 1 ? true : false;
		internal bool? IsFriendlyFlag => this.IsFriendly == 0 ? (bool?)null : this.IsFriendly == 1 ? true : false;
		internal bool? IsNPCFlag => this.IsNPC == 0 ? (bool?)null : this.IsNPC == 1 ? true : false;



		////////////////

		public ProjectileStateDefinition() { }

		public ProjectileStateDefinition( int isHostile, int isFriendly, int isPlayer, int amount ) {
			this.IsHostile = isHostile;
			this.IsFriendly = isFriendly;
			this.IsNPC = isPlayer;
			this.Amount = amount;
		}
	}




	public partial class DestructibleTilesConfig : ModConfig {
		//[JsonIgnore]
		private bool ApplyDefaults = false;



		////////////////

		public override ConfigScope Mode => ConfigScope.ServerSide;



		////////////////

		public DestructibleTilesConfig() {
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
				this.TileDamageScaleOverrides[ TileID.GetUniqueKey( TileID.MartianConduitPlating ) ]
					= new PositiveSingleDefinition { Amount = 0.1f };

				this.ProjectileDamageDefaults[ ProjectileID.GetUniqueKey( ProjectileID.Grenade ) ]
					= new ProjectileStateDefinition( 0, 0, 0, 60 );
				this.ProjectileDamageDefaults[ ProjectileID.GetUniqueKey( ProjectileID.Explosives ) ]
					= new ProjectileStateDefinition( 0, 0, 0, 500 );
				this.ProjectileDamageDefaults[ProjectileID.GetUniqueKey( ProjectileID.GrenadeI )]
					= new ProjectileStateDefinition( 0, 0, 0, 40 );
				this.ProjectileDamageDefaults[ProjectileID.GetUniqueKey( ProjectileID.RocketI )]
					= new ProjectileStateDefinition( 0, 0, 0, 40 );
				this.ProjectileDamageDefaults[ProjectileID.GetUniqueKey( ProjectileID.ProximityMineI )]
					= new ProjectileStateDefinition( 0, 0, 0, 40 );
				this.ProjectileDamageDefaults[ProjectileID.GetUniqueKey( ProjectileID.GrenadeII )]
					= new ProjectileStateDefinition( 0, 0, 0, 40 );
				this.ProjectileDamageDefaults[ProjectileID.GetUniqueKey( ProjectileID.RocketII )]
					= new ProjectileStateDefinition( 0, 0, 0, 40 );
				this.ProjectileDamageDefaults[ProjectileID.GetUniqueKey( ProjectileID.ProximityMineII )]
					= new ProjectileStateDefinition( 0, 0, 0, 40 );
				this.ProjectileDamageDefaults[ProjectileID.GetUniqueKey( ProjectileID.GrenadeIII )]
					= new ProjectileStateDefinition( 0, 0, 0, 65 );
				this.ProjectileDamageDefaults[ProjectileID.GetUniqueKey( ProjectileID.RocketIII )]
					= new ProjectileStateDefinition( 0, 0, 0, 65 );
				this.ProjectileDamageDefaults[ProjectileID.GetUniqueKey( ProjectileID.ProximityMineIII )]
					= new ProjectileStateDefinition( 0, 0, 0, 65 );
				this.ProjectileDamageDefaults[ProjectileID.GetUniqueKey( ProjectileID.GrenadeIV )]
					= new ProjectileStateDefinition( 0, 0, 0, 65 );
				this.ProjectileDamageDefaults[ProjectileID.GetUniqueKey( ProjectileID.RocketIV )]
					= new ProjectileStateDefinition( 0, 0, 0, 65 );
				this.ProjectileDamageDefaults[ProjectileID.GetUniqueKey( ProjectileID.ProximityMineIV )]
					= new ProjectileStateDefinition( 0, 0, 0, 65 );
				this.ProjectileDamageDefaults[ProjectileID.GetUniqueKey( ProjectileID.Landmine )]
					= new ProjectileStateDefinition( 0, 0, 0, 250 );
				this.ProjectileDamageDefaults[ProjectileID.GetUniqueKey( ProjectileID.RocketSnowmanI )]
					= new ProjectileStateDefinition( 0, 0, 0, 40 );
				this.ProjectileDamageDefaults[ProjectileID.GetUniqueKey( ProjectileID.RocketSnowmanII )]
					= new ProjectileStateDefinition( 0, 0, 0, 40 );
				this.ProjectileDamageDefaults[ProjectileID.GetUniqueKey( ProjectileID.RocketSnowmanIII )]
					= new ProjectileStateDefinition( 0, 0, 0, 40 );
				this.ProjectileDamageDefaults[ProjectileID.GetUniqueKey( ProjectileID.RocketSnowmanIV )]
					= new ProjectileStateDefinition( 0, 0, 0, 40 );
				this.ProjectileDamageDefaults[ProjectileID.GetUniqueKey( ProjectileID.StickyGrenade )]
					= new ProjectileStateDefinition( 0, 0, 0, 60 );
				this.ProjectileDamageDefaults[ProjectileID.GetUniqueKey( ProjectileID.BouncyGrenade )]
					= new ProjectileStateDefinition( 0, 0, 0, 65 );
				this.ProjectileDamageDefaults[ProjectileID.GetUniqueKey( ProjectileID.PartyGirlGrenade )]
					= new ProjectileStateDefinition( 0, 0, 0, 30 );

				this.ProjectileDamageDefaults[ProjectileID.GetUniqueKey( ProjectileID.MolotovFire )]
					= new ProjectileStateDefinition( 0, 0, 0, 45 );
				this.ProjectileDamageDefaults[ProjectileID.GetUniqueKey( ProjectileID.MolotovFire2 )]
					= new ProjectileStateDefinition( 0, 0, 0, 45 );
				this.ProjectileDamageDefaults[ProjectileID.GetUniqueKey( ProjectileID.MolotovFire3 )]
					= new ProjectileStateDefinition( 0, 0, 0, 45 );

				this.TileArmor[TileID.GetUniqueKey( TileID.LihzahrdBrick )]
					= new PositiveIntDefinition { Amount = 150 };
			}
		}

		public override ModConfig Clone() {
			var clone = (DestructibleTilesConfig)base.Clone();

			clone.ProjectileDamageDefaults = this.ProjectileDamageDefaults?.ToDictionary( kv => kv.Key, kv => kv.Value );
			clone.ProjectileDamageOverrides = this.ProjectileDamageOverrides?.ToDictionary( kv => kv.Key, kv => kv.Value );
			clone.ProjectilesAsExplosivesAndRadius = this.ProjectilesAsExplosivesAndRadius?.ToDictionary( kv => kv.Key, kv => kv.Value );
			clone.ProjectilesAsConsecutiveHittingAndCooldown = this.ProjectilesAsConsecutiveHittingAndCooldown?.ToDictionary( kv => kv.Key, kv => kv.Value );
			clone.TileDamageScaleOverrides = this.TileDamageScaleOverrides?.ToDictionary( kv => kv.Key, kv => kv.Value );
			clone.TileArmor = this.TileArmor?.ToDictionary( kv => kv.Key, kv => kv.Value );

			return clone;
		}


		////////////////

		public void SetProjectileDefaults() {	//<- Must be called from PostSetupContent
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
