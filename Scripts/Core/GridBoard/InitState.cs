namespace GameForest.Core.GridBoard
{
    public class InitState : State
    {
        public InitState(Board board) : base(board) {}

        public override void SelectCell(Cell cell) {}

        public override void Match(Cell cell) {}
    }
}