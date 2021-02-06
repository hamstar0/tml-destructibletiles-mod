using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using HamstarHelpers.Helpers.Debug;


namespace DestructibleTiles {
	partial class DestructibleTilesProjectile : GlobalProjectile {
		public override void AI( Projectile projectile ) {
			if( projectile.aiStyle == 84 ) {    // <- Beam weapons!
				this.BehaviorAsBeam( projectile );
			}
		}


		////////////////

		public override void Kill( Projectile projectile, int timeLeft ) {
			if( timeLeft > 3 ) { return; }

			var config = DestructibleTilesConfig.Instance;
			var projDef = new ProjectileDefinition( projectile.type );
			bool isExplosive = config.ProjectilesAsAoE.ContainsKey( projDef );

			if( isExplosive ) {
				this.BehaviorAsAoE( projectile );
			}
		}

		////////////////

		public override bool OnTileCollide( Projectile projectile, Vector2 oldVelocity ) {
			var config = DestructibleTilesConfig.Instance;
			var projDef = new ProjectileDefinition( projectile.type );

			// Explosives are handled elsewhere
			if( config.ProjectilesAsAoE.ContainsKey( projDef ) ) {
				return base.OnTileCollide( projectile, oldVelocity );
			}
			
			bool _;
			if( !DestructibleTilesProjectile.CanHitTiles(projectile, out _) ) {
				return base.OnTileCollide( projectile, oldVelocity );
			}

			this.BehaviorAsKinetic( projectile, oldVelocity );

			return base.OnTileCollide( projectile, oldVelocity );
		}
	}
}
