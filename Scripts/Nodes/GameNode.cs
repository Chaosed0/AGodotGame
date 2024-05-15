using System.Diagnostics;
using System.Threading.Tasks;
using EdTestGame.Backend;
using Godot;

namespace EdTestGame.Scripts.Nodes;

public partial class GameNode : Node2D
{
    public static GameNode Instance;
    
    [Export]
    public MapNode map;
    
    [Export]
    public TickRunnerNode tickRunner;
    
    [Export]
    public ScoreCounterNode scoreCounter;
    
    [Export]
    public RestartUiNode restartUi;

    private GameManager _manager;

    public override void _EnterTree()
    {
        if (Instance != null)
        {
            GD.PrintErr($"More than one instance of GameNode in scene!");
        }

        Instance = this;
        _manager = GameManager.Create();
    }

    public void SetRootNode(RootNode node)
    {
        restartUi.SetRootNode(node);
    }

    public void OnPlayerDied()
    {
        tickRunner.Stop();
        restartUi.OnPlayerDied();
    }

    public void OnMonsterDied()
    {
        scoreCounter.OnMonsterDied();
    }

    public override void _ExitTree()
    {
        _manager.Dispose();
        
        if (Instance == this)
        {
            Instance = null;
        }
    }
}