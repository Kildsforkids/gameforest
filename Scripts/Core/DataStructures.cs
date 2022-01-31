namespace GameForest.Core
{
    public struct Vector2Int
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Vector2Int(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }

        public static bool operator ==(Vector2Int v1, Vector2Int v2)
        {
            return (v1.X == v2.X) && (v1.Y == v2.Y);
        }

        public static bool operator !=(Vector2Int v1, Vector2Int v2)
        {
            return (v1.X != v2.X) || (v1.Y != v2.Y);
        }

        public static Vector2Int operator -(Vector2Int v1, Vector2Int v2)
        {
            return new Vector2Int(v1.X - v2.X, v1.Y - v2.Y);
        }
    }

    public enum Direction
    {
        None,
        Up,
        Down,
        Left,
        Right
    }
}