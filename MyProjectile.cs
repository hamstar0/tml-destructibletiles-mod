using System;
using System.Collections.Generic;
using HamstarHelpers.Helpers.TileHelpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;


namespace DestructibleTiles {
	partial class DestructibleTilesProjectile : GlobalProjectile {
		private static object MyLock = new object();



		////////////////
		
		public override bool OnTileCollide( Projectile projectile, Vector2 oldVelocity ) {
			var dim = new Rectangle( (int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height );
			int posX = dim.X + (int)oldVelocity.X;
			int posY = dim.Y + (int)oldVelocity.Y;

			int projRight = posX + dim.Width;
			int projBottom = posY + dim.Height;

			bool onlySometimesRespects;
			bool respectsPlatforms = Helpers.ProjectileHelpers.ProjectileHelpers.RespectsPlatforms( projectile, out onlySometimesRespects )
				&& !onlySometimesRespects;

			IDictionary<int, int> hits = new Dictionary<int, int>();

			for( int i = (posX >> 4); (i<<4) <= projRight; i++ ) {
				for( int j = (posY >> 4); (j<<4) <= projBottom; j++ ) {
					Tile tile = Main.tile[i, j];
					if( TileHelpers.IsAir(tile) ) { continue; }

					if( TileHelpers.IsSolid(tile, respectsPlatforms, false) ) {
						hits[i] = j;
					}
				}
			}
			
			this.HitTiles( projectile, hits );

			return base.OnTileCollide( projectile, oldVelocity );
		}
	}
}
