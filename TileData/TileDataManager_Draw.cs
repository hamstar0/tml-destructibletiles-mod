using HamstarHelpers.Helpers.XnaHelpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;


namespace DestructibleTiles.MultiHitTile {
	public partial class TileDataManager {
		private static void _DrawPostDrawAll( GameTime _ ) {
			var mymod = DestructibleTilesMod.Instance;
			if( mymod == null ) { return; }
			var mngr = mymod.TileDataMngr;
			if( mngr == null ) { return; }

			bool __;
			XnaHelpers.DrawBatch(
				( sb ) => {
					var mymod2 = DestructibleTilesMod.Instance;
					mymod2.TileDataMngr.DrawTileOverlays( sb );
				},
				SpriteSortMode.Deferred,
				BlendState.AlphaBlend,
				Main.DefaultSamplerState,
				DepthStencilState.None,
				Main.instance.Rasterizer,
				null,
				Main.BackgroundViewMatrix.TransformationMatrix,
				out __
			);
		}


		////////////////

		public void DrawTileOverlays( SpriteBatch sb ) {
			/*if( !Main.SettingsEnabled_MinersWobble ) {
				return;
			}*/

			lock( TileDataManager.MyLock ) {
				foreach( var kv in this.Data ) {
					foreach( var kv2 in kv.Value ) {
						kv2.Value.AnimationTimeElapsed++;

						int x = kv.Key;
						int y = kv2.Key;
						Tile tile = Main.tile[x, y];
						TileData data = kv2.Value;

						if( !TileDataManager.IsValidTile(x, y) ) { continue; }
						if( tile.slope() > 0 ) { continue; }
						if( tile.halfBrick() ) { continue; }
						if( TileLoader.IsClosedDoor( tile ) ) { continue; }

						if( tile.type == 5 ) {
							int frameX = (int)( tile.frameX / 22 );
							int frameY = (int)( tile.frameY / 22 );

							if( frameY < 9 ) {
								if( !
									( ( frameX != 1 && frameX != 2 ) || frameY < 6 || frameY > 8 ) &&
									( frameX != 3 || frameY > 2 ) &&
									( frameX != 4 || frameY < 3 || frameY > 5 ) &&
									( frameX != 5 || frameY < 6 || frameY > 8 )
								) {
									continue;
								}
							}
						} else if( tile.type == 72 ) {
							if( tile.frameX > 34 ) {
								continue;
							}
						}

						this.DrawTileOverlay( sb, x, y, tile, data );
					}
				}
			}
		}


		public void DrawTileOverlay( SpriteBatch sb, int x, int y, Tile tile, TileData data ) {
			int crackPercent = 0;
			if( data.Damage >= 80 ) {
				crackPercent = 3;
			} else if( data.Damage >= 60 ) {
				crackPercent = 2;
			} else if( data.Damage >= 40 ) {
				crackPercent = 1;
			} else if( data.Damage >= 20 ) {
				crackPercent = 0;
			}

			var crackFrame = new Rectangle( data.CrackStyle * 18, crackPercent * 18, 16, 16 );
			crackFrame.Inflate( -2, -2 );
			if( tile.type == 5 || tile.type == 72 ) {
				crackFrame.X = ( 4 + data.CrackStyle / 2 ) * 18;
			}

			Color color = Lighting.GetColor( x, y ) * 0.8f;
			float animProgress = (float)data.AnimationTimeElapsed / 10f;
			float animSwellPercent = animProgress % 0.5f;
			animSwellPercent *= 2;
			if( animProgress > 1f ) {
				animSwellPercent = 1f - animSwellPercent;
			}

			Tile tileSafely = Framing.GetTileSafely( x, y );
			Texture2D tileTex;
			if( Main.canDrawColorTile( tileSafely.type, (int)tileSafely.color() ) ) {
				tileTex = Main.tileAltTexture[(int)tileSafely.type, (int)tileSafely.color()];
			} else {
				tileTex = Main.tileTexture[(int)tileSafely.type];
			}

			var origin = new Vector2( 8f );
			var scale = new Vector2( animSwellPercent * 0.2f + 1f );   // animSwellPercent * 0.45f + 1f;?
			Vector2 position = new Vector2(
				(float)( origin.X + ( x * 16 ) - (int)Main.screenPosition.X ),
				(float)( origin.Y + ( y * 16 ) - (int)Main.screenPosition.Y )
			).Floor();

			var texFrame = new Rectangle( (int)tile.frameX, (int)tile.frameY, 16, 16 );

			sb.Draw( tileTex, position, new Rectangle?( texFrame ), color, 0f, origin, scale, SpriteEffects.None, 0f );
			color.A = 180;
			sb.Draw( Main.tileCrackTexture, position, new Rectangle?( crackFrame ), color, 0f, origin, scale, SpriteEffects.None, 0f );
		}
	}
}
