using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using HamstarHelpers.Helpers.Tiles;


namespace DestructibleTiles {
	partial class DestructibleTilesProjectile : GlobalProjectile {
		public static int ComputeProjectileDamage( Projectile projectile ) {
			var config = DestructibleTilesConfig.Instance;
			var projDef = new ProjectileDefinition( projectile.type );

			if( projectile.minion && config.MinionsCannotHitTiles ) {
				return 0;
			}

			if( config.ProjectileTileDamageUltimate.ContainsKey( projDef ) ) {
				ProjectileStateDefinition projDmgOver = config.ProjectileTileDamageUltimate[projDef];

				if( projDmgOver.IsProjectileMatch( projectile ) ) {
					return projDmgOver.Amount;
				}
			}

			if( projectile.damage > 0 ) {
				return (int)( (float)projectile.damage * config.AllDamagesScale );
			}

			if( config.ProjectileTileDamageDefaults.ContainsKey( projDef ) ) {
				ProjectileStateDefinition projDmgDef = config.ProjectileTileDamageDefaults[projDef];

				if( projDmgDef.IsProjectileMatch( projectile ) ) {
					return (int)( (float)projDmgDef.Amount * config.AllDamagesScale );
				}
			}

			return (int)( (float)projectile.damage * config.AllDamagesScale );
		}

		public static int ComputeBeamProjectileDamage( Projectile projectile ) {
			var config = DestructibleTilesConfig.Instance;
			int damage = DestructibleTilesProjectile.ComputeProjectileDamage( projectile );

			return (int)((float)damage * config.BeamDamageScale);
		}


		////

		public static float ComputeHitDamage( Tile tile, int baseDamage, int totalHits ) {
			var config = DestructibleTilesConfig.Instance;
			float dmg = (float)baseDamage / (float)totalHits;
			float scale = 1f;
			string tileName = TileID.GetUniqueKey( tile.type );

			float armor = config.TileArmor.ContainsKey(tileName)
				? (float)config.TileArmor[ tileName ].Amount : 0f;

			if( armor >= dmg ) {
				return 0;
			}
			
			if( config.SpecificTileDamageScales.ContainsKey(tileName) ) {
				scale = config.SpecificTileDamageScales[ tileName ].Amount;
			}

			if( config.UseVanillaTileDamageScalesUnlessOverridden ) {
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
