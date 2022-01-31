using Godot;

namespace GameForest.UI
{
    public class MainMenu : Control
    {
        private void NextScene()
        {
            GetTree().ChangeScene("res://Scenes/Game.tscn");
        }

        private void OnPlayButtonPressed()
        {
            NextScene();
        }
    }
}
