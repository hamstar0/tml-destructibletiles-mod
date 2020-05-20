using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using HamstarHelpers.Classes.Tiles.TilePattern;
using HamstarHelpers.Helpers.Collisions;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Tiles;
using HamstarHelpers.Helpers.Projectiles.Attributes;
using HamstarHelpers.Services.Timers;


namespace DestructibleTiles {
	partial class DestructibleTilesProjectile : GlobalProjectile {
		private static object MyLock = new object();



		////////////////

		public static bool CanHitTiles( Projectile projectile, out bool hasCooldown ) {
			var mymod = DestructibleTilesMod.Instance;
			string projName = ProjectileID.GetUniqueKey( projectile.type );
			string timerName = "PTH_" + projectile.whoAmI;
			bool isConsecutive = Timers.GetTimerTickDuration( timerName ) > 0;
			
			hasCooldown = mymod.Config.ProjectilesAsConsecutiveHittingAndCooldown.ContainsKey( projName );

			Timers.SetTimer( timerName, 2, false, () => false );

			if( isConsecutive ) {
				if( hasCooldown ) {
					ProjectileStateDefinition projConsec = mymod.Config.ProjectilesAsConsecutiveHittingAndCooldown[projName];

					if( projConsec.IsFriendlyFlag.HasValue && projectile.friendly == projConsec.IsFriendlyFlag.Value ) {
						if( projConsec.IsHostileFlag.HasValue && projectile.hostile == projConsec.IsHostileFlag.Value ) {
							if( projConsec.IsNPCFlag.HasValue && projectile.npcProj == projConsec.IsNPCFlag.Value ) {
								string repeatTimerName = timerName + "_repeat";
								int cooldown = mymod.Config.ProjectilesAsConsecutiveHittingAndCooldown[projName].Amount;

								if( Timers.GetTimerTickDuration( repeatTimerName ) <= 0 ) {
									Timers.SetTimer( repeatTimerName, cooldown, false, () => false );
									isConsecutive = false;
								}
							}
						}
					}
				}
			}

			return !isConsecutive;
		}



		////////////////

		public override void AI( Projectile projectile ) {
			if( projectile.aiStyle == 84 ) {    // <- Beam weapons!
				bool hasCooldown;

				if( DestructibleTilesProjectile.CanHitTiles(projectile, out hasCooldown) || !hasCooldown ) {
					Vector2 projPos = projectile.Center + (projectile.velocity * projectile.localAI[1]);
					Point? tilePosNull = TileFinderHelpers.GetNearestTile( projPos, TilePattern.CommonSolid, 32 );

//DebugHelpers.Print("proj_"+projectile.whoAmI,
//	"vel: "+projectile.velocity.X.ToString("N2")+":"+projectile.velocity.Y.ToString("N2")+
//	", ai: "+string.Join(", ", projectile.ai.Select(f=>f.ToString("N1")))+
//	", localAi: "+string.Join(", ", projectile.localAI.Select(f=>f.ToString("N1"))),
//	20 );
//var rpos1 = (projPos / 16f) * 16f;
//var rpos2 = new Vector2( rpos1.X + 16, rpos1.Y + 16 );
//Dust.QuickBox( rpos1, rpos2, 0, Color.Blue, d => { } );
					if( tilePosNull.HasValue ) {
						var tilePos = tilePosNull.Value;
						int damage = DestructibleTilesProjectile.ComputeBeamProjectileDamage( projectile );

						if( DestructibleTilesProjectile.HitTile(damage, tilePos.X, tilePos.Y, 1) ) {
							bool _;
							projectile.localAI[1] = TileCollisionHelpers.MeasureWorldDistanceToTile( projectile.Center, projectile.velocity, 2400f, out _ );
						}
//var pos1 = tilePos.ToVector2() * 16f;
//var pos2 = new Vector2( pos1.X + 16, pos1.Y + 16 );
//Dust.QuickBox( pos1, pos2, 0, Color.Red, d => { } );
					}
				}
			}
		}


		////////////////

		public override void Kill( Projectile projectile, int timeLeft ) {
			if( timeLeft > 3 ) { return; }

			var mymod = DestructibleTilesMod.Instance;
			string projName = ProjectileID.GetUniqueKey( projectile.type );
			bool isExplosive = mymod.Config.ProjectilesAsExplosivesAndRadius.ContainsKey( projName );

			if( isExplosive ) {
				ProjectileStateDefinition projExploDef = mymod.Config.ProjectilesAsExplosivesAndRadius[projName];

				if( projExploDef.IsFriendlyFlag.HasValue && projectile.friendly == projExploDef.IsFriendlyFlag.Value ) {
					if( projExploDef.IsHostileFlag.HasValue && projectile.hostile == projExploDef.IsHostileFlag.Value ) {
						if( projExploDef.IsNPCFlag.HasValue && projectile.npcProj == projExploDef.IsNPCFlag.Value ) {
							int tileX = (int)projectile.position.X >> 4;
							int tileY = (int)projectile.position.Y >> 4;
							int damage = DestructibleTilesProjectile.ComputeProjectileDamage( projectile );

							if( mymod.Config.DebugModeInfo ) {
								Main.NewText( "RADIUS - " + projectile.Name + "(" + projName + "), radius:" + projExploDef.Amount + ", damage:" + damage );
							}

							DestructibleTilesProjectile.HitTilesInRadius( tileX, tileY, projExploDef.Amount, damage );
						}
					}
				}
			}
		}

		////////////////

		public override bool OnTileCollide( Projectile projectile, Vector2 oldVelocity ) {
			var mymod = DestructibleTilesMod.Instance;
			string projName = ProjectileID.GetUniqueKey( projectile.type );

			// Explosives are handled elsewhere
			if( mymod.Config.ProjectilesAsExplosivesAndRadius.ContainsKey( projName ) ) {
				return base.OnTileCollide( projectile, oldVelocity );
			}
			
			bool _;
			if( DestructibleTilesProjectile.CanHitTiles(projectile, out _) ) {
				var rect = new Rectangle( (int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height );
				rect.X += (int)oldVelocity.X;
				rect.Y += (int)oldVelocity.Y;
				
				bool onlySometimesRespects;
				bool respectsPlatforms = ProjectileAttributeHelpers.DoesVanillaProjectileHitPlatforms( projectile, out onlySometimesRespects )
					&& !onlySometimesRespects;
				
				int damage = DestructibleTilesProjectile.ComputeProjectileDamage( projectile );
				
				TilePattern tilePattern = new TilePattern(
					new TilePatternBuilder {
						HasSolidProperties = true,
						IsPlatform = respectsPlatforms
					}
				);
				//true, respectsPlatforms, false, null, null, null );
				IDictionary<int, int> hits = TileFinderHelpers.GetTilesInWorldRectangle( rect, tilePattern );
				
				if( hits.Count == 0 ) {
					Point? point = TileFinderHelpers.GetNearestTile( projectile.Center, tilePattern, 32 );
					if( point.HasValue ) {
						hits[ point.Value.X ] = point.Value.Y;
					}
				}

				if( mymod.Config.DebugModeInfo ) {
					Main.NewText( "RECTANGLE - " + projectile.Name + " hits #" + hits.Count + " tiles" );
				}

				foreach( var xy in hits ) {
					DestructibleTilesProjectile.HitTile( damage, xy.Key, xy.Value, hits.Count );
				}
			}

			return base.OnTileCollide( projectile, oldVelocity );
		}
	}
}
