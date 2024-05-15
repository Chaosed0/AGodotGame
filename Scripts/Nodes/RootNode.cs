using Godot;

namespace EdTestGame.Scripts.Nodes;

public partial class RootNode : Node2D
{
    [Export]
    public PackedScene gameScene;

    private GameNode _currentGame;

    public override void _Ready()
    {
        RestartGame();
    }

    public void RestartGame()
    {
        RemoveChild(_currentGame);
        _currentGame?.QueueFree();
        _currentGame = gameScene.Instantiate<GameNode>();
        _currentGame.SetRootNode(this);
        AddChild(_currentGame);
    }
}