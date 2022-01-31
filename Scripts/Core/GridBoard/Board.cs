using Godot;
using System.Collections.Generic;
using System.Linq;

namespace GameForest.Core.GridBoard
{
	public class Board : Node2D
	{
		[Export] private int width;
		[Export] private int height;
		[Export] private float offset;
		[Export] private PackedScene cell;
		[Export] private PackedScene[] tileVariants;

		public int Width => width;
		public int Height => height;
		public Cell this[int x, int y] => cells[x, y];

		private Cell[,] cells;
		private State state;

		public override void _Ready()
		{
			GD.Randomize();
			ChangeState(new InitState(this));
			Initialize();
		}

		public override void _Process(float delta)
		{
			if (Input.IsActionJustPressed("ui_accept")) {
				ChangeState(new SelectState(this));
			}
		}

		public void SelectCell(Cell cell)
		{
			state.SelectCell(cell);
		}

		public void Match(Cell cell)
		{
			state.Match(cell);
		}

		public void ChangeState(State state)
		{
			this.state = state;
		}

		private void Initialize()
		{
			cells = new Cell[width, height];
			for (int y = 0; y < height; y++) {
				for (int x = 0; x < width; x++) {
					var cellInstance = cell.Instance<Cell>();
					AddChild(cellInstance);
					cellInstance.Board = this;
					cellInstance.Index = new Vector2Int(x, y);
					cellInstance.Position = Position + new Vector2(x, -y) * offset;
					cells[x, y] = cellInstance;
					cellInstance.CreateTile(GetRandomNoMatchingTile(cellInstance));
				}
			}
			ChangeState(new SelectState(this));
		}

		private Queue<Cell> VerticalMatch(Cell cell, Tile tile, Direction direction = Direction.None)
		{
			var index = cell.Index;
			Queue<Cell> matchedCells = new Queue<Cell>(7);
			if (direction != Direction.Up) {
				for (int x = index.X, y = index.Y - 1; y >= 0; y--) {
					if (!cells[x, y].IsTileEqual(tile)) {
						break;
					}
					matchedCells.Enqueue(cells[x, y]);
				}
			}
			if (direction != Direction.Down) {
				for (int x = index.X, y = index.Y + 1; y < height; y++) {
					if (!cells[x, y].IsTileEqual(tile)) {
						break;
					}
					matchedCells.Enqueue(cells[x, y]);
				}
			}
			return matchedCells;
		}

		private Queue<Cell> HorizontalMatch(Cell cell, Tile tile, Direction direction = Direction.None)
		{
			var index = cell.Index;
			Queue<Cell> matchedCells = new Queue<Cell>(7);
			if (direction != Direction.Right) {
				for (int x = index.X - 1, y = index.Y; x >= 0; x--) {
					if (!cells[x, y].IsTileEqual(tile)) {
						break;
					}
					matchedCells.Enqueue(cells[x, y]);
				}
			}
			if (direction != Direction.Left) {
				for (int x = index.X + 1, y = index.Y; x < width; x++) {
					if (!cells[x, y].IsTileEqual(tile)) {
						break;
					}
					matchedCells.Enqueue(cells[x, y]);
				}
			}
			return matchedCells;
		}

        public Tile GetRandomNoMatchingTile(Cell cell)
		{
			var index = cell.Index;
			var randomTile = GetRandomTile(out int randomIndex);
			var variants = Enumerable.Range(0, tileVariants.Length).ToList();
			if (index.X > 1 && HorizontalMatch(cell, randomTile, Direction.Left).Count > 1) {
				variants.Remove(randomIndex);
				int nextRandom = (int)(GD.Randi() % variants.Count);
				randomTile = tileVariants[variants[nextRandom]].Instance<Tile>();
			}
			if (index.Y > 1 && VerticalMatch(cell, randomTile, Direction.Down).Count > 1) {
				variants.Remove(randomIndex);
				int nextRandom = (int)(GD.Randi() % variants.Count);
				randomTile = tileVariants[variants[nextRandom]].Instance<Tile>();
			}
			return randomTile;
		}

		private Tile GetRandomTile(out int randomIndex)
		{
			randomIndex = (int)(GD.Randi() % tileVariants.Length);
			return tileVariants[randomIndex].Instance<Tile>();
		}
	}
}
