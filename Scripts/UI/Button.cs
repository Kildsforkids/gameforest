using Godot;

namespace GameForest.UI
{
    public class Button : Godot.Button
    {
        private Sprite sprite;
        private Color spriteColor;

        public override void _Ready()
        {
            sprite = GetNode<Sprite>("Sprite");
            spriteColor = sprite.Modulate;
        }

        private void OnMouseEntered()
        {
            sprite.Modulate = Color.ColorN("white");
        }

        private void OnMouseExited()
        {
            sprite.Modulate = spriteColor;
        }
    }
}
