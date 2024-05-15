using System.Diagnostics;
using System.Threading.Tasks;
using EdTestGame.Backend;
using Godot;

namespace EdTestGame.Scripts.Nodes;

public partial class RootNode : Node2D
{
    public static RootNode Instance;
    
    [Export]
    public MapNode map;
    
    [Export]
    public TickRunnerNode tickRunner;

    public override void _EnterTree()
    {
        if (Instance != null)
        {
            GD.PrintErr($"More than one instance of RootNode in scene!");
        }

        Instance = this;
        GameManager.Create();
    }

    public void OnPlayerDied()
    {
        tickRunner.Stop();
    }

    public override void _ExitTree()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}