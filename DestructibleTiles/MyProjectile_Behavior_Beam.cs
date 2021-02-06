using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Classes.Tiles.TilePattern;
using HamstarHelpers.Helpers.Collisions;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Tiles;


namespace DestructibleTiles {
	partial class DestructibleTilesProjectile : GlobalProjectile {
		public void BehaviorAsBeam( Projectile projectile ) {
			bool hasCooldown;
			if( !DestructibleTilesProjectile.CanHitTiles(projectile, out hasCooldown) || hasCooldown ) {
				return;
			}

			Vector2 projPos = projectile.Center + (projectile.velocity * projectile.localAI[1]);
			Point? tilePosNull = TileFinderHelpers.GetNearestTile( projPos, TilePattern.CommonSolid, 32 );
			if( !tilePosNull.HasValue ) {
				return;
			}

//DebugHelpers.Print("proj_"+projectile.whoAmI,
//	"vel: "+projectile.velocity.X.ToString("N2")+":"+projectile.velocity.Y.ToString("N2")+
//	", ai: "+string.Join(", ", projectile.ai.Select(f=>f.ToString("N1")))+
//	", localAi: "+string.Join(", ", projectile.localAI.Select(f=>f.ToString("N1"))),
//	20 );
//var rpos1 = (projPos / 16f) * 16f;
//var rpos2 = new Vector2( rpos1.X + 16, rpos1.Y + 16 );
//Dust.QuickBox( rpos1, rpos2, 0, Color.Blue, d => { } );
			var tilePos = tilePosNull.Value;
			int damage = DestructibleTilesProjectile.ComputeBeamProjectileDamage( projectile );

			if( DestructibleTilesProjectile.HitTile(damage, tilePos.X, tilePos.Y, 1) ) {
				bool _;
				projectile.localAI[1] = TileCollisionHelpers.MeasureWorldDistanceToTile( projectile.Center, projectile.velocity, 2400f, out _ );
//var pos1 = tilePos.ToVector2() * 16f;
//var pos2 = new Vector2( pos1.X + 16, pos1.Y + 16 );
//Dust.QuickBox( pos1, pos2, 0, Color.Red, d => { } );
			}
		}
	}
}
