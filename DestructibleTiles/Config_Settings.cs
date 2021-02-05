using System;
using System.Collections.Generic;
using System.ComponentModel;
using Terraria.ModLoader.Config;
using HamstarHelpers.Helpers.TModLoader.Configs;


namespace DestructibleTiles {
	public partial class DestructibleTilesConfig : ModConfig {
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
		[Range( 0f, 1000f )]
		[DefaultValue(1f)]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float AllDamagesScale = 1f;


		[Label( "Projectile damage to tiles (accepts scaling)" )]
		public Dictionary<string, ProjectileStateDefinition>	ProjectileDamageDefaults =
			new Dictionary<string, ProjectileStateDefinition>();

		[Label( "Projectile damage to tiles (overrides all)" )]
		public Dictionary<string, ProjectileStateDefinition>	ProjectileDamageOverrides =
			new Dictionary<string, ProjectileStateDefinition>();

		[Label( "Explosive projectives with their radiuses" )]
		public Dictionary<string, ProjectileStateDefinition>	ProjectilesAsExplosivesAndRadius =
			new Dictionary<string, ProjectileStateDefinition>();

		[Label( "Consecutive-hitting projectiles with their cooldowns" )]
		public Dictionary<string, ProjectileStateDefinition>	ProjectilesAsConsecutiveHittingAndCooldown =
			new Dictionary<string, ProjectileStateDefinition>();
		//public IDictionary<string, float>	ProjectilesAsPhysicsObjectsAndMaxVelocity = new Dictionary<string, float>();


		[Label( "Tile damage multiplier" )]
		public Dictionary<string, PositiveSingleDefinition>	TileDamageScaleOverrides =
			new Dictionary<string, PositiveSingleDefinition>();

		[Label( "Tile armor" )]
		public Dictionary<string, PositiveIntDefinition>		TileArmor =
			new Dictionary<string, PositiveIntDefinition>();


		[Label( "Beam damage scale" )]
		[Range( 0f, 1000f )]
		[DefaultValue( 1f / 30f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float BeamDamageScale = 1f / 30f;
	}
}
