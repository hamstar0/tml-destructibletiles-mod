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


		[Label( "Auto-load settings for explosive projectiles (below)" )]
		[Tooltip( "Required to get correct explosives settings; may override user settings" )]
		[DefaultValue( true )]
		public bool AutoLoadDefaultExplosiveProjectiles { get; set; } = true;


		[Label( "Destroyed tiles drop their items" )]
		public bool DestroyedTilesDropItems { get; set; } = false;


		[Label( "Tiles resist damage as if pickaxed" )]
		[Tooltip( "Overrides user settings below" )]
		[DefaultValue( true )]
		public bool UseVanillaTileDamageScalesUnlessOverridden { get; set; } = true;

		//

		[Label( "Projectile damage amount multiplier against tiles" )]
		[Range( 0f, 1000f )]
		[DefaultValue( 1f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float AllDamagesScale { get; set; } = 1f;


		[Label( "Projectile damage amount to any tile" )]
		[Tooltip( "Overrides all other settings" )]
		public Dictionary<ProjectileDefinition, ProjectileStateDefinition> ProjectileTileDamageUltimate { get; set; } =
			new Dictionary<ProjectileDefinition, ProjectileStateDefinition>();

		[Label( "Backup projectile damage to any tile (accepts scaling)" )]
		[Tooltip( "Used only when a projectile does not indicate its own damage amount" )]
		public Dictionary<ProjectileDefinition, ProjectileStateDefinition> ProjectileTileDamageDefaults { get; set; } =
			new Dictionary<ProjectileDefinition, ProjectileStateDefinition> {
				{ new ProjectileDefinition( ProjectileID.Grenade ),			new ProjectileStateDefinition( 0, 0, 0, 60 ) },
				{ new ProjectileDefinition( ProjectileID.Explosives ),		new ProjectileStateDefinition( 0, 0, 0, 500 ) },
				{ new ProjectileDefinition( ProjectileID.GrenadeI ),		new ProjectileStateDefinition( 0, 0, 0, 40 ) },
				{ new ProjectileDefinition( ProjectileID.RocketI ),			new ProjectileStateDefinition( 0, 0, 0, 40 ) },
				{ new ProjectileDefinition( ProjectileID.ProximityMineI ),	new ProjectileStateDefinition( 0, 0, 0, 40 ) },
				{ new ProjectileDefinition( ProjectileID.GrenadeII ),		new ProjectileStateDefinition( 0, 0, 0, 40 ) },
				{ new ProjectileDefinition( ProjectileID.RocketII ),		new ProjectileStateDefinition( 0, 0, 0, 40 ) },
				{ new ProjectileDefinition( ProjectileID.ProximityMineII ),	new ProjectileStateDefinition( 0, 0, 0, 40 ) },
				{ new ProjectileDefinition( ProjectileID.GrenadeIII ),		new ProjectileStateDefinition( 0, 0, 0, 65 ) },
				{ new ProjectileDefinition( ProjectileID.RocketIII ),		new ProjectileStateDefinition( 0, 0, 0, 65 ) },
				{ new ProjectileDefinition( ProjectileID.ProximityMineIII ),new ProjectileStateDefinition( 0, 0, 0, 65 ) },
				{ new ProjectileDefinition( ProjectileID.GrenadeIV ),		new ProjectileStateDefinition( 0, 0, 0, 65 ) },
				{ new ProjectileDefinition( ProjectileID.RocketIV ),		new ProjectileStateDefinition( 0, 0, 0, 65 ) },
				{ new ProjectileDefinition( ProjectileID.ProximityMineIV ),	new ProjectileStateDefinition( 0, 0, 0, 65 ) },
				{ new ProjectileDefinition( ProjectileID.Landmine ),		new ProjectileStateDefinition( 0, 0, 0, 250 ) },
				{ new ProjectileDefinition( ProjectileID.RocketSnowmanI ),	new ProjectileStateDefinition( 0, 0, 0, 40 ) },
				{ new ProjectileDefinition( ProjectileID.RocketSnowmanII ),	new ProjectileStateDefinition( 0, 0, 0, 40 ) },
				{ new ProjectileDefinition( ProjectileID.RocketSnowmanIII ),new ProjectileStateDefinition( 0, 0, 0, 40 ) },
				{ new ProjectileDefinition( ProjectileID.RocketSnowmanIV ),	new ProjectileStateDefinition( 0, 0, 0, 40 ) },
				{ new ProjectileDefinition( ProjectileID.StickyGrenade ),	new ProjectileStateDefinition( 0, 0, 0, 60 ) },
				{ new ProjectileDefinition( ProjectileID.BouncyGrenade ),	new ProjectileStateDefinition( 0, 0, 0, 65 ) },
				{ new ProjectileDefinition( ProjectileID.PartyGirlGrenade ),new ProjectileStateDefinition( 0, 0, 0, 30 ) },

				{ new ProjectileDefinition( ProjectileID.MolotovFire ),		new ProjectileStateDefinition( 0, 0, 0, 45 ) },
				{ new ProjectileDefinition( ProjectileID.MolotovFire2 ),	new ProjectileStateDefinition( 0, 0, 0, 45 ) },
				{ new ProjectileDefinition( ProjectileID.MolotovFire3 ),	new ProjectileStateDefinition( 0, 0, 0, 45 ) }
			};

		[Label( "Projectiles to treat as explosives, and their radiuses" )]
		[Tooltip( "Tile damage applied only when projectile expires (AKA explodes)" )]
		public Dictionary<ProjectileDefinition, ProjectileStateDefinition> ProjectilesAsAoE { get; set; } =
			new Dictionary<ProjectileDefinition, ProjectileStateDefinition>();

		[Label( "Projectiles that damage tiles repeatedly, and their cooldowns" )]
		public Dictionary<ProjectileDefinition, ProjectileStateDefinition> ProjectilesAsConsecutiveHitters { get; set; } =
			new Dictionary<ProjectileDefinition, ProjectileStateDefinition>();
		//public IDictionary<string, float>	ProjectilesAsPhysicsObjectsAndMaxVelocity = new Dictionary<string, float>();

		//

		[Label( "Tile damage multiplier" )]
		[Tooltip( "Tile definition = `TileID.GetUniqueKey(id)` value" )]
		public Dictionary<string, PositiveSingleDefinition> SpecificTileDamageScales { get; set; } =
			new Dictionary<string, PositiveSingleDefinition> {
				{ TileID.GetUniqueKey( TileID.MartianConduitPlating ), new PositiveSingleDefinition { Amount = 0.1f } }
			};

		[Label( "Tile armor (flat amount subtracted from damage)" )]
		[Tooltip( "Tile definition = `TileID.GetUniqueKey(id)` value" )]
		public Dictionary<string, PositiveIntDefinition> TileArmor { get; set; } =
			new Dictionary<string, PositiveIntDefinition> {
				{ TileID.GetUniqueKey( TileID.LihzahrdBrick ), new PositiveIntDefinition { Amount = 150 } }
			};


		[Label( "Beam damage multiplier" )]
		[Range( 0f, 1000f )]
		[DefaultValue( 1f / 30f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float BeamDamageScale { get; set; } = 1f / 30f;

		[Label( "Summoned minions cannot hit tiles" )]
		[DefaultValue( true )]
		public bool MinionsCannotHitTiles { get; set; } = true;
	}
}
