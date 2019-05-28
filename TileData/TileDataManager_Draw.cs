using HamstarHelpers.Helpers.DebugHelpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;


namespace DestructibleTiles.MultiHitTile {
	public partial class TileDataManager {
		public void DrawTileOverlays( SpriteBatch sb ) {
			foreach( var kv in this.Data ) {
				foreach( var kv2 in kv.Value ) {
					int x = kv.Key;
					int y = kv2.Key;

					if( this.CanDrawTileOverlay( x, y ) ) {
						this.DrawTileOverlay( sb, x, y );
					}
				}
			}
		}

		public bool CanDrawTileOverlay( int tileX, int tileY ) {
			/*if( !Main.SettingsEnabled_MinersWobble ) {
				return;
			}*/
			
			Tile tile = Main.tile[tileX, tileY];
			TileData data = this.Data[tileX][tileY];

			if( data.AnimationTimeDuration > 0 ) {
				data.AnimationTimeDuration--;
			}

			if( HamstarHelpers.Helpers.TileHelpers.TileHelpers.IsAir(tile) ) { return false; }
			if( !TileDataManager.IsValidTile(tileX, tileY) ) { return false; }
			if( tile.slope() > 0 ) { return false; }
			if( tile.halfBrick() ) { return false; }
			if( TileLoader.IsClosedDoor( tile ) ) { return false; }

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
						return false;
					}
				}
			} else if( tile.type == 72 ) {
				if( tile.frameX > 34 ) {
					return false;
				}
			}

			return true;
		}


		public void DrawTileOverlay( SpriteBatch sb, int tileX, int tileY ) {
			Tile tile = Main.tile[ tileX, tileY ];
			TileData data = this.Data[tileX][tileY];

			int crackStage = 0;
			if( data.Damage >= 80 ) {
				crackStage = 3;
			} else if( data.Damage >= 60 ) {
				crackStage = 2;
			} else if( data.Damage >= 40 ) {
				crackStage = 1;
			} else if( data.Damage >= 20 ) {
				crackStage = 0;
			}

			var crackFrame = new Rectangle( data.CrackStyle * 18, crackStage * 18, 16, 16 );
			crackFrame.Inflate( -2, -2 );
			if( tile.type == 5 || tile.type == 72 ) {
				crackFrame.X = (4 + (data.CrackStyle / 2)) * 18;
			}

			var origin = new Vector2( 8f );
			var scale = new Vector2( 1f );
			var position = new Vector2(
				(float)( origin.X + (tileX * 16) - (int)Main.screenPosition.X ),
				(float)( origin.Y + (tileY * 16) - (int)Main.screenPosition.Y )
			).Floor();
			Color color = Lighting.GetColor( tileX, tileY ) * 0.8f;

			if( data.AnimationTimeDuration > 0 ) {
				Texture2D tileTex;
				if( Main.canDrawColorTile( tile.type, (int)tile.color() ) ) {
					tileTex = Main.tileAltTexture[(int)tile.type, (int)tile.color()];
				} else {
					tileTex = Main.tileTexture[(int)tile.type];
				}
				if( tileTex == null ) {
					return;
				}

				var texFrame = new Rectangle( (int)tile.frameX, (int)tile.frameY, 16, 16 );

				float animProgress = (float)data.AnimationTimeDuration / TileDataManager.HitAnimationMaxDuration;
				float zoom = (animProgress % 0.5f) * 2;
				if( animProgress >= 0.5f ) {
					zoom = 1f - zoom;
				}

				scale = new Vector2( zoom * 0.2f + 1f );   // animSwellPercent * 0.45f + 1f;?

				sb.Draw( tileTex, position, new Rectangle?( texFrame ), color, 0f, origin, scale, SpriteEffects.None, 0f );
			}

			color.A = 180;
			if( Main.tileCrackTexture != null ) {
				sb.Draw( Main.tileCrackTexture, position, new Rectangle?( crackFrame ), color, 0f, origin, scale, SpriteEffects.None, 0f );
			}
		}
	}
}
