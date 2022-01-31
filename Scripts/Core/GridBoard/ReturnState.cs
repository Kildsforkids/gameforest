namespace GameForest.Core.GridBoard
{
    public class ReturnState : State
    {
        private bool isCompleted;

        public ReturnState(Board board) : base(board) {}

        public override void SelectCell(Cell cell) {}

        public override void Match(Cell cell)
        {
            if (!isCompleted) {
                isCompleted = true;
                return;
            }
            Board.ChangeState(new SelectState(Board));
        }
    }
}