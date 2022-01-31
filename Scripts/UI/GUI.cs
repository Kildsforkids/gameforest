using Godot;

namespace GameForest.UI
{
    public class GUI : Control
    {
        private Label scoreLabel;
        private Label timerLabel;

        public override void _Ready()
        {
            scoreLabel = GetNode<Label>("ScoreLabel");
            timerLabel = GetNode<Label>("TimerLabel");
        }

        public void SetScore(int score)
        {
            scoreLabel.Text = score.ToString();
        }

        public void SetTime(int seconds)
        {
            timerLabel.Text = seconds.ToString();
        }
    }
}
