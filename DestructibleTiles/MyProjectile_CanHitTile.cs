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
			bool isConsecutive = Timers.GetTimerTickDuration( timerName ) > 0;
			
			hasCooldown = config.ProjectilesAsConsecutiveHitters.ContainsKey( projDef );

			Timers.SetTimer( timerName, 2, false, () => false );

			if( isConsecutive ) {
				if( hasCooldown ) {
					ProjectileStateDefinition projConsec = config.ProjectilesAsConsecutiveHitters[projDef];

					if( projConsec.IsProjectileMatch(projectile) ) {
						string repeatTimerName = timerName + "_repeat";
						int cooldown = config.ProjectilesAsConsecutiveHitters[projDef].Amount;

						if( Timers.GetTimerTickDuration( repeatTimerName ) <= 0 ) {
							Timers.SetTimer( repeatTimerName, cooldown, false, () => false );
							isConsecutive = false;
						}
					}
				}
			}

			return !isConsecutive;
		}
	}
}
