using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.Graphics.Effects;


namespace DestructibleTiles {
	class TileEffectsOverlay : Overlay {
		public Vector2 TargetPosition = Vector2.Zero;



		////////////////

		public TileEffectsOverlay() : base( EffectPriority.VeryHigh, RenderLayers.TilesAndNPCs ) { }


		////////////////

		public override void Activate( Vector2 position, params object[] args ) {
			this.TargetPosition = position;
			this.Mode = OverlayMode.FadeIn;
		}

		public override void Deactivate( params object[] args ) {
			this.Mode = OverlayMode.FadeOut;
		}

		public override bool IsVisible() {
			return true;
		}


		////////////////

		public override void Draw( SpriteBatch sb ) {
			DestructibleTilesMod.Instance.TileDataMngr.DrawTileOverlays( sb );
			//sb.Draw( this._texture.Value, new Rectangle( 0, 0, Main.screenWidth, Main.screenHeight ), Main.bgColor );
		}

		public override void Update( GameTime _ ) { }
	}
}
