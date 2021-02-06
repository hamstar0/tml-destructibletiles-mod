using System;
using System.Collections.Generic;
using System.ComponentModel;
using Terraria.ID;
using Terraria.ModLoader.Config;
using HamstarHelpers.Helpers.TModLoader.Configs;


namespace DestructibleTiles {
	public partial class DestructibleTilesConfig : ModConfig {
		[Label( "Debug Mode" )]
		[Tooltip( "Logs or displays developer-relevant information." )]
		public bool DebugModeInfo { get; set; } = false;


		[Label( "Auto-load explosive projectiles into below settings" )]
		[Tooltip( "Required to perform explosive damage calculations properly; may override user settings" )]
		[DefaultValue( true )]
		public bool AutoLoadDefaultExplosiveProjectiles { get; set; } = true;


		[Label( "Destroyed tiles drop block items" )]
		public bool DestroyedTilesDropItems { get; set; } = false;


		[Label( "Use vanilla tile damage scaling (unless overridden)" )]
		[DefaultValue( true )]
		public bool UseVanillaTileDamageScalesUnlessOverridden { get; set; } = true;


		[Label( "Damage multiplier for all projectiles" )]
		[Range( 0f, 1000f )]
		[DefaultValue( 1f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float AllDamagesScale { get; set; } = 1f;


		[Label( "Projectile damage to tiles (overrides all)" )]
		public Dictionary<ProjectileDefinition, ProjectileStateDefinition> ProjectileTileDamageOverrides { get; set; } =
			new Dictionary<ProjectileDefinition, ProjectileStateDefinition>();

		[Label( "Substitute projectile damage to tiles (accepts scaling)" )]
		[Tooltip( "Used only when a projectile does not indicate its own damage amount, and is not overridden" )]
		public Dictionary<ProjectileDefinition, ProjectileStateDefinition> ProjectileTileDamageDefaults { get; set; } =
			new Dictionary<ProjectileDefinition, ProjectileStateDefinition> {
				{ new ProjectileDefinition( ProjectileID.Grenade ), new ProjectileStateDefinition( 0, 0, 0, 60 ) },
				{ new ProjectileDefinition( ProjectileID.Explosives ), new ProjectileStateDefinition( 0, 0, 0, 500 ) },
				{ new ProjectileDefinition( ProjectileID.GrenadeI ), new ProjectileStateDefinition( 0, 0, 0, 40 ) },
				{ new ProjectileDefinition( ProjectileID.RocketI ), new ProjectileStateDefinition( 0, 0, 0, 40 ) },
				{ new ProjectileDefinition( ProjectileID.ProximityMineI ), new ProjectileStateDefinition( 0, 0, 0, 40 ) },
				{ new ProjectileDefinition( ProjectileID.GrenadeII ), new ProjectileStateDefinition( 0, 0, 0, 40 ) },
				{ new ProjectileDefinition( ProjectileID.RocketII ), new ProjectileStateDefinition( 0, 0, 0, 40 ) },
				{ new ProjectileDefinition( ProjectileID.ProximityMineII ), new ProjectileStateDefinition( 0, 0, 0, 40 ) },
				{ new ProjectileDefinition( ProjectileID.GrenadeIII ), new ProjectileStateDefinition( 0, 0, 0, 65 ) },
				{ new ProjectileDefinition( ProjectileID.RocketIII ), new ProjectileStateDefinition( 0, 0, 0, 65 ) },
				{ new ProjectileDefinition( ProjectileID.ProximityMineIII ), new ProjectileStateDefinition( 0, 0, 0, 65 ) },
				{ new ProjectileDefinition( ProjectileID.GrenadeIV ), new ProjectileStateDefinition( 0, 0, 0, 65 ) },
				{ new ProjectileDefinition( ProjectileID.RocketIV ), new ProjectileStateDefinition( 0, 0, 0, 65 ) },
				{ new ProjectileDefinition( ProjectileID.ProximityMineIV ), new ProjectileStateDefinition( 0, 0, 0, 65 ) },
				{ new ProjectileDefinition( ProjectileID.Landmine ), new ProjectileStateDefinition( 0, 0, 0, 250 ) },
				{ new ProjectileDefinition( ProjectileID.RocketSnowmanI ), new ProjectileStateDefinition( 0, 0, 0, 40 ) },
				{ new ProjectileDefinition( ProjectileID.RocketSnowmanII ), new ProjectileStateDefinition( 0, 0, 0, 40 ) },
				{ new ProjectileDefinition( ProjectileID.RocketSnowmanIII ), new ProjectileStateDefinition( 0, 0, 0, 40 ) },
				{ new ProjectileDefinition( ProjectileID.RocketSnowmanIV ), new ProjectileStateDefinition( 0, 0, 0, 40 ) },
				{ new ProjectileDefinition( ProjectileID.StickyGrenade ), new ProjectileStateDefinition( 0, 0, 0, 60 ) },
				{ new ProjectileDefinition( ProjectileID.BouncyGrenade ), new ProjectileStateDefinition( 0, 0, 0, 65 ) },
				{ new ProjectileDefinition( ProjectileID.PartyGirlGrenade ), new ProjectileStateDefinition( 0, 0, 0, 30 ) },

				{ new ProjectileDefinition( ProjectileID.MolotovFire ), new ProjectileStateDefinition( 0, 0, 0, 45 ) },
				{ new ProjectileDefinition( ProjectileID.MolotovFire2 ), new ProjectileStateDefinition( 0, 0, 0, 45 ) },
				{ new ProjectileDefinition( ProjectileID.MolotovFire3 ), new ProjectileStateDefinition( 0, 0, 0, 45 ) }
			};

		[Label( "Explosive projectives with their radiuses" )]
		public Dictionary<ProjectileDefinition, ProjectileStateDefinition> ProjectilesAsAoE { get; set; } =
			new Dictionary<ProjectileDefinition, ProjectileStateDefinition>();

		[Label( "Consecutive-hitting projectiles with their cooldowns" )]
		public Dictionary<ProjectileDefinition, ProjectileStateDefinition> ProjectilesAsConsecutiveHitters { get; set; } =
			new Dictionary<ProjectileDefinition, ProjectileStateDefinition>();
		//public IDictionary<string, float>	ProjectilesAsPhysicsObjectsAndMaxVelocity = new Dictionary<string, float>();


		[Label( "Tile damage multiplier" )]
		public Dictionary<string, PositiveSingleDefinition> SpecificTileDamageScales { get; set; } =
			new Dictionary<string, PositiveSingleDefinition> {
				{ TileID.GetUniqueKey( TileID.MartianConduitPlating ), new PositiveSingleDefinition { Amount = 0.1f } }
			};

		[Label( "Tile armor" )]
		public Dictionary<string, PositiveIntDefinition> TileArmor { get; set; } =
			new Dictionary<string, PositiveIntDefinition> {
				{ TileID.GetUniqueKey( TileID.LihzahrdBrick ), new PositiveIntDefinition { Amount = 150 } }
			};


		[Label( "Beam damage scale" )]
		[Range( 0f, 1000f )]
		[DefaultValue( 1f / 30f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float BeamDamageScale { get; set; } = 1f / 30f;

		[DefaultValue( true )]
		public bool MinionsCannotHitTiles { get; set; } = true;



		////

		[Header("Deactivated Settings (backup recovery only)")]
		[Label( "(Unused; old) Projectile damage to tiles" )]
		public Dictionary<string, ProjectileStateDefinition> ProjectileDamageDefaults { get; set; } =
			new Dictionary<string, ProjectileStateDefinition>();

		[Label( "(Unused; old) Projectile damage to tiles (overrides all)" )]
		public Dictionary<string, ProjectileStateDefinition> ProjectileDamageOverrides { get; set; } =
			new Dictionary<string, ProjectileStateDefinition>();

		[Label( "(Unused; old) Explosive projectives with their radiuses" )]
		public Dictionary<string, ProjectileStateDefinition> ProjectilesAsExplosivesAndRadius { get; set; } =
			new Dictionary<string, ProjectileStateDefinition>();

		[Label( "(Unused; old) Consecutive-hitting projectiles with their cooldowns" )]
		public Dictionary<string, ProjectileStateDefinition> ProjectilesAsConsecutiveHittingAndCooldown { get; set; } =
			new Dictionary<string, ProjectileStateDefinition>();
		//public IDictionary<string, float>	ProjectilesAsPhysicsObjectsAndMaxVelocity = new Dictionary<string, float>();


		[Label( "(Unused; old) Tile damage multiplier" )]
		public Dictionary<string, PositiveSingleDefinition> TileDamageScaleOverrides { get; set; } =
			new Dictionary<string, PositiveSingleDefinition>();
	}
}
