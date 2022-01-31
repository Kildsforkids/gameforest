using Godot;
using GameForest.UI;
using GameForest.Core.GridBoard;

namespace GameForest.Core
{
    public class Game : Node
    {
        public static Game instance { get; private set; }

        private GUI gui;
        private Timer gameOverTimer;
        private PopupDialog messagePanel;
        private Board board;
        private int score;

        public override void _Ready()
        {
            instance = this;
            gui = GetNode<GUI>("CanvasLayer/GUI");
            gameOverTimer = GetNode<Timer>("GameOverTimer");
            messagePanel = GetNode<PopupDialog>("CanvasLayer/MessagePanel");
            board = GetNode<Board>("Board");
            UpdateScore(0);
        }

        public override void _Process(float delta)
        {
            UpdateTimer();
        }

        public void AddScore(int amount)
        {
            score += amount;
            gui.SetScore(score);
        }

        public void UpdateScore(int score)
        {
            this.score = score;
            gui.SetScore(score);
        }

        private void OnGameOver()
        {
            board.ChangeState(new InitState(board));
            messagePanel.Show();
        }

        private void UpdateTimer()
        {
            gui.SetTime((int)gameOverTimer.TimeLeft);
        }

        private void OnMessagePanelClose()
        {
            GetTree().ChangeScene("res://Scenes/MainMenu.tscn");
        }
    }
}
