using System;


namespace DestructibleTiles.MultiHitTile {
	public class TileData {
		public int Damage;
		public int TileType;
		public int TTL;
		public int CrackStyle;
		public int AnimationTimeElapsed;
		//public Vector2 AnimationDirection;



		////////////////

		public TileData() {
			this.Clear();
		}

		public void Clear() {
			this.Damage = 0;
			this.TileType = 0;
			this.TTL = 0;

			TileDataManager.RerollCrackStyle( this );
		}
	}
}
