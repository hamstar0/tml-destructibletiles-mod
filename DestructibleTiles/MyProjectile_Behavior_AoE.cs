using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using HamstarHelpers.Helpers.Debug;


namespace DestructibleTiles {
	partial class DestructibleTilesProjectile : GlobalProjectile {
		public void BehaviorAsAoE( Projectile projectile ) {
			var config = DestructibleTilesConfig.Instance;
			var projDef = new ProjectileDefinition( projectile.type );

			ProjectileStateDefinition projExploDef = config.ProjectilesAsAoE[projDef];
			if( !projExploDef.IsProjectileMatch(projectile) ) {
				return;
			}

			int tileX = (int)projectile.position.X >> 4;
			int tileY = (int)projectile.position.Y >> 4;
			int damage = DestructibleTilesProjectile.ComputeProjectileDamage( projectile );

			if( config.DebugModeInfo ) {
				Main.NewText( "RADIUS - " + projDef.ToString() + ", radius:" + projExploDef.Amount + ", damage:" + damage );
			}

			DestructibleTilesProjectile.HitTilesInRadius( tileX, tileY, projExploDef.Amount, damage );
		}
	}
}
