using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Classes.Tiles.TilePattern;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Tiles;
using HamstarHelpers.Helpers.Projectiles.Attributes;
using HamstarHelpers.Services.Timers;


namespace DestructibleTiles {
	partial class DestructibleTilesProjectile : GlobalProjectile {
		public static IList<(ushort x, ushort y)> FindNearbyHittableTiles(
					Vector2 position,
					Rectangle hitArea,
					bool respectsPlatforms ) {
			var tilePattern = new TilePattern(
				new TilePatternBuilder {
					IsActive = true,
					HasSolidProperties = true,
					IsPlatform = respectsPlatforms
				}
			); //true, respectsPlatforms, false, null, null, null );

			IList<(ushort, ushort)> hits = TileFinderHelpers.GetTileMatchesInWorldRectangle( hitArea, tilePattern );

			if( hits.Count == 0 ) {
				Point? point = TileFinderHelpers.GetNearestTile( position, tilePattern, 32 );
				if( point.HasValue ) {
					hits.Add( ((ushort)point.Value.X, (ushort)point.Value.Y) );
				}
			}

			return hits;
		}



		////////////////

		public void BehaviorAsKinetic( Projectile projectile, Vector2 oldVelocity ) {
			var config = DestructibleTilesConfig.Instance;

			var rect = new Rectangle(
				(int)projectile.position.X + (int)oldVelocity.X,
				(int)projectile.position.Y + (int)oldVelocity.Y,
				projectile.width,
				projectile.height
			);

			bool onlySometimesRespects;
			bool respectsPlatforms = ProjectileAttributeHelpers.DoesVanillaProjectileHitPlatforms(
					projectile,
					out onlySometimesRespects
				) && !onlySometimesRespects;

			int damage = DestructibleTilesProjectile.ComputeProjectileDamage( projectile );

			IList<(ushort, ushort)> hits = DestructibleTilesProjectile.FindNearbyHittableTiles(
				projectile.Center,
				rect,
				respectsPlatforms
			);

			if( config.DebugModeInfo ) {
				this.OutputKineticProjectileDebugInfo( projectile, oldVelocity, hits );
			}

			foreach( (ushort x, ushort y) in hits ) {
				DestructibleTilesProjectile.HitTile( damage, x, y, hits.Count );
			}
		}


		////////////////

		private void OutputKineticProjectileDebugInfo(
					Projectile projectile,
					Vector2 oldVelocity,
					IList<(ushort, ushort)> hits ) {
			Vector2 projPos = projectile.position;
			int projWid = projectile.width;
			int projHei = projectile.height;

			int t = 60;
			Timers.RunUntil( () => {
				Vector2 rectPos = projPos + oldVelocity;
				Dust.QuickBox( rectPos, rectPos + new Vector2( projWid, projHei ), 8, Color.Red, d => { } );
				Dust.QuickDust( projPos, Color.Blue );

				foreach( (ushort x, ushort y) in hits ) {
					Dust.QuickDust( new Vector2( ( x * 16 ) + 8, ( y * 16 ) + 8 ), Color.Green );
				}

				return t-- > 0;
			}, true );

			Main.NewText( "RECTANGLE - " + projectile.Name + " hits #" + hits.Count + " tiles" );
		}
	}
}
