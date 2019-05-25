using System;
using Terraria;
using Terraria.ModLoader;


namespace DestructibleTiles.Helpers.TileHelpers {
	public class TileHelpers {
		public static float GetDamageScale( Tile tile, float baseDamage, out bool isAbsolute ) {
			isAbsolute = false;
			float scale = 0f;

			if( Main.tileNoFail[(int)tile.type] ) {
				isAbsolute = true;
			}
			if( Main.tileDungeon[(int)tile.type] || tile.type == 25 || tile.type == 58 || tile.type == 117 || tile.type == 203 ) {
				scale = 1f / 2f;
			} else if( tile.type == 48 || tile.type == 232 ) {
				scale = 1f / 4f;
			} else if( tile.type == 226 ) {
				scale = 1f / 4f;
			} else if( tile.type == 107 || tile.type == 221 ) {
				scale = 1f / 2f;
			} else if( tile.type == 108 || tile.type == 222 ) {
				scale = 1f / 3f;
			} else if( tile.type == 111 || tile.type == 223 ) {
				scale = 1f / 4f;
			} else if( tile.type == 211 ) {
				scale = 1f / 5f;
			} else {
				int moddedDamage = 0;
				TileLoader.MineDamage( (int)baseDamage, ref moddedDamage );

				scale = (float)moddedDamage / baseDamage;
			}

			if( tile.type == 211 && baseDamage < 200 ) {
				scale = 0f;
			}
			if( ( tile.type == 25 || tile.type == 203 ) && baseDamage < 65 ) {
				scale = 0f;
			} else if( tile.type == 117 && baseDamage < 65 ) {
				scale = 0f;
			} else if( tile.type == 37 && baseDamage < 50 ) {
				scale = 0f;
			} else if( tile.type == 404 && baseDamage < 65 ) {
				scale = 0f;
			} else if( ( tile.type == 22 || tile.type == 204 ) && /*(double)y > Main.worldSurface &&*/ baseDamage < 55 ) {
				scale = 0f;
			} else if( tile.type == 56 && baseDamage < 65 ) {
				scale = 0f;
			} else if( tile.type == 58 && baseDamage < 65 ) {
				scale = 0f;
			} else if( ( tile.type == 226 || tile.type == 237 ) && baseDamage < 210 ) {
				scale = 0f;
			} else if( Main.tileDungeon[(int)tile.type] && baseDamage < 65 ) {
				//if( (double)x < (double)Main.maxTilesX * 0.35 || (double)x > (double)Main.maxTilesX * 0.65 ) {
				//	scale = 0f;
				//}
				scale = 0f;
			} else if( tile.type == 107 && baseDamage < 100 ) {
				scale = 0f;
			} else if( tile.type == 108 && baseDamage < 110 ) {
				scale = 0f;
			} else if( tile.type == 111 && baseDamage < 150 ) {
				scale = 0f;
			} else if( tile.type == 221 && baseDamage < 100 ) {
				scale = 0f;
			} else if( tile.type == 222 && baseDamage < 110 ) {
				scale = 0f;
			} else if( tile.type == 223 && baseDamage < 150 ) {
				scale = 0f;
			} else {
				if( TileLoader.GetTile(tile.type) != null ) {
					int moddedDamage = 0;
					TileLoader.PickPowerCheck( tile, (int)baseDamage, ref moddedDamage );
					
					scale = (float)moddedDamage / baseDamage;
				} else {
					scale = 1f;
				}
			}

			if( tile.type == 147 || tile.type == 0 || tile.type == 40 || tile.type == 53 || tile.type == 57 || tile.type == 59 || tile.type == 123 || tile.type == 224 || tile.type == 397 ) {
				scale = 1f;
			}
			if( tile.type == 165 || Main.tileRope[(int)tile.type] || tile.type == 199 || Main.tileMoss[(int)tile.type] ) {
				isAbsolute = true;
			}

			return scale;
			//if( this.hitTile.AddDamage( tileId, scale, false ) >= 100 && ( tile.type == 2 || tile.type == 23 || tile.type == 60 || tile.type == 70 || tile.type == 109 || tile.type == 199 || Main.tileMoss[(int)tile.type] ) ) {
			//	scale = 0f;
			//}
			//if( tile.type == 128 || tile.type == 269 ) {
			//	if( tile.frameX == 18 || tile.frameX == 54 ) {
			//		x--;
			//		tile = Main.tile[x, y];
			//		this.hitTile.UpdatePosition( tileId, x, y );
			//	}
			//	if( tile.frameX >= 100 ) {
			//		scale = 0f;
			//		Main.blockMouse = true;
			//	}
			//}
			//if( tile.type == 334 ) {
			//	if( tile.frameY == 0 ) {
			//		y++;
			//		tile = Main.tile[x, y];
			//		this.hitTile.UpdatePosition( tileId, x, y );
			//	}
			//	if( tile.frameY == 36 ) {
			//		y--;
			//		tile = Main.tile[x, y];
			//		this.hitTile.UpdatePosition( tileId, x, y );
			//	}
			//	int i = (int)tile.frameX;
			//	bool flag = i >= 5000;
			//	bool flag2 = false;
			//	if( !flag ) {
			//		int num2 = i / 18;
			//		num2 %= 3;
			//		x -= num2;
			//		tile = Main.tile[x, y];
			//		if( tile.frameX >= 5000 ) {
			//			flag = true;
			//		}
			//	}
			//	if( flag ) {
			//		i = (int)tile.frameX;
			//		int num3 = 0;
			//		while( i >= 5000 ) {
			//			i -= 5000;
			//			num3++;
			//		}
			//		if( num3 != 0 ) {
			//			flag2 = true;
			//		}
			//	}
			//	if( flag2 ) {
			//		scale = 0f;
			//		Main.blockMouse = true;
			//	}
			//}
		}
	}
}
