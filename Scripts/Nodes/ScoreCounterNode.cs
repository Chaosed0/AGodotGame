using Godot;

namespace EdTestGame.Scripts.Nodes;

public partial class ScoreCounterNode : Label
{
    private int _score;

    public override void _Ready()
    {
        this.Text = "0";
    }

    public void OnMonsterDied()
    {
        this.Text = (++_score).ToString();
    }
}