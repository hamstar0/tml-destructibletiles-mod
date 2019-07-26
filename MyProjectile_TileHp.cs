using HamstarHelpers.Helpers.Projectiles;
using HamstarHelpers.Helpers.TileHelpers;
using HamstarHelpers.Helpers.Tiles;
using System;
using Terraria;
using Terraria.ModLoader;


namespace DestructibleTiles {
	partial class DestructibleTilesProjectile : GlobalProjectile {
		public static int ComputeProjectileDamage( Projectile projectile ) {
			var mymod = DestructibleTilesMod.Instance;
			string projName = ProjectileIdentityHelpers.GetUniqueKey( projectile.type );

			if( mymod.Config.ProjectileDamageOverrides.ContainsKey( projName ) ) {
				ProjectileStateDefinition projDmgOver = mymod.Config.ProjectileDamageOverrides[projName];

				if( projDmgOver.IsFriendlyFlag.HasValue && projectile.friendly == projDmgOver.IsFriendlyFlag.Value ) {
					if( projDmgOver.IsHostileFlag.HasValue && projectile.hostile == projDmgOver.IsHostileFlag.Value ) {
						if( projDmgOver.IsNPCFlag.HasValue && projectile.npcProj == projDmgOver.IsNPCFlag.Value ) {
							return mymod.Config.ProjectileDamageOverrides[projName].Amount;
						}
					}
				}
			}

			if( projectile.damage > 0 ) {
				return (int)( (float)projectile.damage * mymod.Config.AllDamagesScale );
			}

			if( mymod.Config.ProjectileDamageDefaults.ContainsKey( projName ) ) {
				ProjectileStateDefinition projDmgDef = mymod.Config.ProjectileDamageDefaults[projName];

				if( projDmgDef.IsFriendlyFlag.HasValue && projectile.friendly == projDmgDef.IsFriendlyFlag.Value ) {
					if( projDmgDef.IsHostileFlag.HasValue && projectile.hostile == projDmgDef.IsHostileFlag.Value ) {
						if( projDmgDef.IsNPCFlag.HasValue && projectile.npcProj == projDmgDef.IsNPCFlag.Value ) {
							return (int)((float)projDmgDef.Amount * mymod.Config.AllDamagesScale);
						}
					}
				}
			}

			return (int)( (float)projectile.damage * mymod.Config.AllDamagesScale );
		}

		public static int ComputeBeamProjectileDamage( Projectile projectile ) {
			var mymod = DestructibleTilesMod.Instance;
			int damage = DestructibleTilesProjectile.ComputeProjectileDamage( projectile );

			return (int)((float)damage * mymod.Config.BeamDamageScale);
		}


		////

		public static float ComputeHitDamage( Tile tile, int baseDamage, int totalHits ) {
			var mymod = DestructibleTilesMod.Instance;
			float dmg = (float)baseDamage / (float)totalHits;
			float scale = 1f;
			string tileName = TileIdentityHelpers.GetUniqueKey( tile.type );

			float armor = mymod.Config.TileArmor.ContainsKey(tileName)
				? (float)mymod.Config.TileArmor[ tileName ].Amount : 0f;

			if( armor >= dmg ) {
				return 0;
			}
			
			if( mymod.Config.TileDamageScaleOverrides.ContainsKey(tileName) ) {
				scale = mymod.Config.TileDamageScaleOverrides[ tileName ].Amount;
			}

			if( mymod.Config.UseVanillaTileDamageScalesUnlessOverridden ) {
				bool isAbsolute;
				scale *= TileHelpers.GetDamageScale( tile, dmg, out isAbsolute );

				if( isAbsolute ) {
					return 100f;	// max
				}
			}

			return (dmg - armor) * scale;
		}
	}
}
