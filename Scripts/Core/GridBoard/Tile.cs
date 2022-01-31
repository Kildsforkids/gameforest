using Godot;

namespace GameForest.Core.GridBoard
{
    public class Tile : Node2D
    {
        [Export] private TileType type;

        public TileType Type => type;

        private Tween tween;
        private Cell cell;

        public override void _Ready()
        {
            tween = GetNode<Tween>("Tween");
            ScaleUp();
        }

        public void Move(Cell cell)
        {
            this.cell = cell;
            tween.InterpolateProperty(
                this,
                "position",
                Position,
                cell.Position,
                .5f
            );
            tween.Start();
        }

        public virtual void Destroy()
        {
            QueueFree();
        }

        private void ScaleUp()
        {
            tween.InterpolateProperty(
                this,
                "scale",
                Vector2.Zero,
                Vector2.One,
                .75f
            );
            tween.Start();
        }

        private void OnTweenCompleted(object obj, NodePath key)
        {
            if (key == ":position") {
                cell.TileSwapped();
            }
        }
    }
}

public enum TileType
{
    None,
    Apple,
    Bread,
    Coconut,
    Milk,
    Orange
}
