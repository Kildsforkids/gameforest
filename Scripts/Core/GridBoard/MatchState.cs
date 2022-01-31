using System.Collections.Generic;
using Godot;

namespace GameForest.Core.GridBoard
{
	public class MatchState : State
	{
		private bool isMatchCompleted;
		private bool wasMatched;
		private Cell firstCell;
		private int collapsedCount = 0;

		public MatchState(Board board) : base(board) {}

		public override void SelectCell(Cell cell) {}

		public override void Match(Cell cell)
		{
			var matchedCells = AllMatch(cell);
			if (matchedCells.Count > 1) {
				ClearLine(matchedCells);
				wasMatched = true;
			}
			if (firstCell != null) {
				if (!wasMatched) {
					cell.SwapTile(firstCell);
					Board.ChangeState(new ReturnState(Board));
				} else {
					CollapseColumns();
				}
				firstCell = null;
			} else {
				firstCell = cell;
			}
		}

		private List<Cell> AllMatch(Cell cell)
		{
			var matchedCells = new List<Cell>(16);
			var horizontalMatch = HorizontalMatch(cell);
			var verticalMatch = VerticalMatch(cell);
			if (horizontalMatch.Count > 1) {
				if (horizontalMatch.Count > 2) {
					GD.Print("LineBonus");
				}
				matchedCells.AddRange(horizontalMatch);
			}
			if (verticalMatch.Count > 1) {
				if (verticalMatch.Count > 2) {
					GD.Print("LineBonus");
				}
				matchedCells.AddRange(verticalMatch);
			}
			if (horizontalMatch.Count > 2 && verticalMatch.Count > 2) {
				GD.Print("BombBonus");
			}
			matchedCells.Add(cell);
			return matchedCells;
		}

		private Queue<Cell> VerticalMatch(Cell cell, Direction direction = Direction.None)
		{
			var index = cell.Index;
			Queue<Cell> matchedCells = new Queue<Cell>(7);
			if (direction != Direction.Up) {
				for (int x = index.X, y = index.Y - 1; y >= 0; y--) {
					if (!Board[x, y].IsTileEqual(cell.Tile)) {
						break;
					}
					matchedCells.Enqueue(Board[x, y]);
				}
			}
			if (direction != Direction.Down) {
				for (int x = index.X, y = index.Y + 1; y < Board.Height; y++) {
					if (!Board[x, y].IsTileEqual(cell.Tile)) {
						break;
					}
					matchedCells.Enqueue(Board[x, y]);
				}
			}
			return matchedCells;
		}

		private Queue<Cell> HorizontalMatch(Cell cell, Direction direction = Direction.None)
		{
			var index = cell.Index;
			Queue<Cell> matchedCells = new Queue<Cell>(7);
			if (direction != Direction.Right) {
				for (int x = index.X - 1, y = index.Y; x >= 0; x--) {
					if (!Board[x, y].IsTileEqual(cell.Tile)) {
						break;
					}
					matchedCells.Enqueue(Board[x, y]);
				}
			}
			if (direction != Direction.Left) {
				for (int x = index.X + 1, y = index.Y; x < Board.Width; x++) {
					if (!Board[x, y].IsTileEqual(cell.Tile)) {
						break;
					}
					matchedCells.Enqueue(Board[x, y]);
				}
			}
			return matchedCells;
		}

		private void ClearLine(List<Cell> match)
		{
			foreach (var cell in match) {
				cell.Clear();
				Game.instance.AddScore(1);
			}
		}

		private Cell GetUpperCellWithTile(Vector2Int index)
		{
			for (int x = index.X, y = index.Y; y < Board.Height; y++) {
				if (!Board[x, y].IsEmpty()) {
					return Board[x, y];
				}  
			}
			return null;
		}

		private void CollapseColumn(Vector2Int index)
		{
			for (int x = index.X, y = index.Y; y < Board.Height; y++) {
				if (!Board[x, y].IsEmpty()) continue;
				var upperCell = GetUpperCellWithTile(new Vector2Int(x, y));
				if (upperCell != null) {
					upperCell.SwapTile(Board[x, y]);
				} else {
					upperCell = Board[x, 7];
					upperCell.CreateTile(Board.GetRandomNoMatchingTile(Board[x, y]));
					upperCell.SwapTile(Board[x, y]);
				}
				collapsedCount++;
			}
		}

		private void CollapseColumns()
		{
			for (int x = 0; x < Board.Width; x++) {
				CollapseColumn(new Vector2Int(x, 0));
			}
			collapsedCount -= 2;
			if (collapsedCount <= 0) {
				Board.ChangeState(new SelectState(Board));
			}
		}
	}
}
