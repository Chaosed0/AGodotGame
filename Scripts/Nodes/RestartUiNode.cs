using Godot;

namespace EdTestGame.Scripts.Nodes;

public partial class RestartUiNode : Button
{
    private RootNode _root;

    public override void _Ready()
    {
        this.Modulate = this.Modulate with { A = 0 };
        this.Pressed += OnRestart;
    }

    public void SetRootNode(RootNode root)
    {
        _root = root;
    }

    public void OnPlayerDied()
    {
        GetTree().CreateTimer(2.0).Timeout += ShowSelf;
    }

    private void ShowSelf()
    {
        this.Modulate = this.Modulate with { A = 1 };
    }

    private void OnRestart()
    {
        _root.RestartGame();
    }
}