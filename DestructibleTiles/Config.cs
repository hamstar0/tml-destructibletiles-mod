using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using HamstarHelpers.Classes.UI.ModConfig;
using HamstarHelpers.Helpers.DotNET.Extensions;


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


		////////////////

		public bool IsProjectileMatch( Projectile proj ) {
			if( !this.IsFriendlyFlag.HasValue || proj.friendly != this.IsFriendlyFlag.Value ) {
				return false;
			}
			if( !this.IsHostileFlag.HasValue || proj.hostile != this.IsHostileFlag.Value ) {
				return false;
			}
			if( !this.IsNPCFlag.HasValue || proj.npcProj != this.IsNPCFlag.Value ) {
				return false;
			}
			return true;
		}
	}




	public partial class DestructibleTilesConfig : ModConfig {
		public static DestructibleTilesConfig Instance => ModContent.GetInstance<DestructibleTilesConfig>();



		////////////////

		public override ConfigScope Mode => ConfigScope.ServerSide;



		////////////////

		public DestructibleTilesConfig() { }

		public override ModConfig Clone() {
			var clone = (DestructibleTilesConfig)base.Clone();

			clone.ProjectileTileDamageDefaults = this.ProjectileTileDamageDefaults?.ToDictionary( kv => kv.Key, kv => kv.Value );
			clone.ProjectileTileDamageOverrides = this.ProjectileTileDamageOverrides?.ToDictionary( kv => kv.Key, kv => kv.Value );
			clone.ProjectilesAsAoE = this.ProjectilesAsAoE?.ToDictionary( kv => kv.Key, kv => kv.Value );
			clone.ProjectilesAsConsecutiveHitters = this.ProjectilesAsConsecutiveHitters?.ToDictionary( kv => kv.Key, kv => kv.Value );
			clone.SpecificTileDamageScales = this.SpecificTileDamageScales?.ToDictionary( kv => kv.Key, kv => kv.Value );
			clone.TileArmor = this.TileArmor?.ToDictionary( kv => kv.Key, kv => kv.Value );

			return clone;
		}


		////////////////

		public void SetProjectileDefaults() {	//<- Must be called from PostSetupContent?
			if( !this.AutoLoadDefaultExplosiveProjectiles ) {
				return;
			}

			IDictionary<ProjectileDefinition, (int radius, int damage)> explosiveProjs
				= DestructibleTilesProjectile.GetExplosivesStats();

			foreach( (ProjectileDefinition projDef, (int radius, int damage) stats) in explosiveProjs ) {
				if( !this.ProjectilesAsAoE.ContainsKey( projDef ) ) {
					this.ProjectilesAsAoE[projDef] = new ProjectileStateDefinition( 0, 0, 0, stats.radius );
				}
				if( !this.ProjectileTileDamageDefaults.ContainsKey( projDef ) ) {
					this.ProjectileTileDamageDefaults[projDef] = new ProjectileStateDefinition( 0, 0, 0, stats.damage );
				}
			}
		}
	}
}
