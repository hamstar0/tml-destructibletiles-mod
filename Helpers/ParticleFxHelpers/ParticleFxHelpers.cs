using Microsoft.Xna.Framework;
using System;
using Terraria;


namespace DestructibleTiles.Helpers.ParticleHelpers {
	public class ParticleFxHelpers {
		public static void MakeDustCloud( Vector2 position, int quantity, float spreadScale=1f, float scale=1f ) {
			Vector2 pos = new Vector2( position.X - 10, position.Y - 10 );

			for( int i = 0; i < quantity; i++ ) {
				int goreType = Main.rand.Next( 61, 64 );
				int goreIdx = Gore.NewGore( pos, default( Vector2 ), goreType, scale );
				Gore gore = Main.gore[goreIdx];
				
				gore.velocity *= spreadScale * 0.3f;
				gore.velocity.X = gore.velocity.X + (float)Main.rand.Next( -10, 11 ) * 0.05f;
				gore.velocity.Y = gore.velocity.Y + (float)Main.rand.Next( -10, 11 ) * 0.05f;
			}
		}
	}
}
