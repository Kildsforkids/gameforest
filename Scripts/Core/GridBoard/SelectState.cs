namespace GameForest.Core.GridBoard
{
    public class SelectState : State
    {
        private Cell firstCell;

        public SelectState(Board board) : base(board) {}

        public override void SelectCell(Cell cell)
        {
			if (firstCell == null) {
				firstCell = cell;
				firstCell.Select();
			} else {
				if (cell.IsSelected) {
					ClearSelectedCell();
					return;
				}
				if (!firstCell.IsNeighbour(cell)) {
					ClearSelectedCell();
					return;
				}
				firstCell.SwapTile(cell);
				ClearSelectedCell();
                Board.ChangeState(new MatchState(Board));
			}
        }

        public override void Match(Cell cell) {}

        private void ClearSelectedCell()
		{
			firstCell?.Deselect();
			firstCell = null;
		}
    }
}