using Godot;

namespace GameForest.Core.GridBoard
{
	public class Cell : Area2D
	{
		public Board Board { get; set; }
		public Vector2Int Index { get; set; }
		public bool IsSelected { get; private set; }
		public Tile Tile { get; private set; }

		private ColorRect mask;

		public override void _Ready()
		{
			mask = GetNode<ColorRect>("Mask");
		}

		public void CreateTile(Tile tile)
		{
			Board.AddChild(tile);
			tile.Position = Position;
			Tile = tile;
		}

		public void SwapTile(Cell cell)
		{
			var tile = cell.Tile;
			cell.MoveTileToCell(Tile);
			MoveTileToCell(tile);
		}

		public void TileSwapped()
		{
			Board.Match(this);
		}

		public void Select()
		{
			mask.Show();
			IsSelected = true;
		}

		public void Deselect()
		{
			mask.Hide();
			IsSelected = false;
		}

		public void Clear()
		{
			if (Tile == null) return;
			Tile.Destroy();
			Tile = null;
		}

		public bool IsEqual(Cell cell)
		{
			if (cell == null) {
				return false;
			}
			return Index == cell.Index;
		}

		public bool IsNeighbour(Cell cell)
		{
			var offset = Index - cell.Index;
			if ((offset.X == 0 && Mathf.Abs(offset.Y) < 2) ||
				(offset.Y == 0 && Mathf.Abs(offset.X) < 2)) {
					return true;
				}
			return false;
		}

		public bool IsTileEqual(Tile tile)
		{
			if (tile == null || Tile == null) {
				return false;
			}
			return Tile.Type == tile.Type;
		}

		public bool IsEmpty()
		{
			return Tile == null;
		}

		public void MoveTileToCell(Tile tile)
		{
			Tile = tile;
			if (tile != null) {
				tile.Move(this);
			}
		}

		private void OnInput(Node viewport, InputEvent @event, int shapeIdx)
		{
			if (@event is InputEventMouseButton mouseButtonEvent) {
				if (mouseButtonEvent.ButtonIndex == 1 && mouseButtonEvent.Pressed && Tile != null) {
					Board.SelectCell(this);
				}
			}
		}
	}
}

