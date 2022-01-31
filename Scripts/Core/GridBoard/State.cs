namespace GameForest.Core.GridBoard
{
    public abstract class State
    {
        protected Board Board;

        public State(Board board)
        {
            Board = board;
        }

        public abstract void SelectCell(Cell cell);
        public abstract void Match(Cell cell);
    }
}