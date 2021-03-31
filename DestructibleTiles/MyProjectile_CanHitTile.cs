using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Services.Timers;


namespace DestructibleTiles {
	partial class DestructibleTilesProjectile : GlobalProjectile {
		public static bool CanHitTiles( Projectile projectile, out bool hasCooldown ) {
			var config = DestructibleTilesConfig.Instance;
			var projDef = new ProjectileDefinition( projectile.type );

			string timerName = "PTH_" + projectile.type + "_" + projectile.whoAmI;
			bool isRepeatHit = Timers.GetTimerTickDuration( timerName ) > 0;
			
			hasCooldown = config.ProjectilesAsConsecutiveHitters.ContainsKey( projDef );

			Timers.SetTimer( timerName, 2, false, () => false );

			if( isRepeatHit ) {
				if( hasCooldown ) {
					DestructibleTilesProjectile.CanHitTilesAgain( projectile, projDef, timerName, ref isRepeatHit );
				}
			}

			return !isRepeatHit;
		}


		private static void CanHitTilesAgain(
					Projectile projectile,
					ProjectileDefinition projDef,
					string timerName,
					ref bool isHit ) {
			var config = DestructibleTilesConfig.Instance;
			ProjectileStateDefinition projConsec = config.ProjectilesAsConsecutiveHitters[ projDef ];

			if( !projConsec.IsProjectileMatch(projectile) ) {
				return;
			}

			string repeatTimerName = timerName + "_repeat";
			int cooldown = config.ProjectilesAsConsecutiveHitters[projDef].Amount;

			if( Timers.GetTimerTickDuration( repeatTimerName ) <= 0 ) {
				Timers.SetTimer( repeatTimerName, cooldown, false, () => false );
				isHit = false;
			}
		}
	}
}
