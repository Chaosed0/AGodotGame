using System.Threading.Tasks;
using EdTestGame.Backend;
using Godot;

namespace EdTestGame.Scripts.Nodes;

public partial class TickRunnerNode : Node2D
{
    [Export] public double tickTime = 1f;

    [Export] public StringName leftAction;
    [Export] public StringName rightAction;
    [Export] public StringName upAction;
    [Export] public StringName downAction;

    [Export] public AudioStreamPlayer audioPlayer;
    
    private Task _startTask;
    private double tickAccumulator;
    private double _timeBegin;
    private double _timeSinceLastTick;
    private bool _enabled = true;

    public override void _Ready()
    {
        _startTask = GameManager.Instance.session.Start();
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed(upAction))
        {
            GameManager.Instance.session.nextPlayerInput = new Vector2I(0, -1);
        }
        else if (@event.IsActionPressed(leftAction))
        {
            GameManager.Instance.session.nextPlayerInput = new Vector2I(-1, 0);
        }
        else if (@event.IsActionPressed(rightAction))
        {
            GameManager.Instance.session.nextPlayerInput = new Vector2I(1, 0);
        }
        else if (@event.IsActionPressed(downAction))
        {
            GameManager.Instance.session.nextPlayerInput = new Vector2I(0, 1);
        }

        if (_timeSinceLastTick < 0.12)
        {
            GameManager.Instance.session.TryHandleLatePlayerInput();
        }
    }

    public void Stop()
    {
        _enabled = false;
        audioPlayer.Stop();
    }

    public override void _Process(double delta)
    {
        if (!_enabled)
        {
            return;
        }

        if (!_startTask.IsCompleted)
        {
            return;
        }
        else if (_startTask.Exception != null)
        {
            GD.PrintErr($"Exception while starting! {_startTask.Exception}");
            return;
        }

        if (!audioPlayer.Playing)
        {
            _timeBegin = Time.GetTicksUsec() + (AudioServer.GetTimeToNextMix() + AudioServer.GetOutputLatency()) * 1000000.0;
            audioPlayer.Play();
            tickAccumulator = tickTime;
        }

        if (Time.GetTicksUsec() < _timeBegin)
        {
            return;
        }
        
        _timeSinceLastTick += delta;

        if (tickAccumulator > tickTime)
        {
            tickAccumulator -= tickTime;
            GameManager.Instance.session.Tick();
            _timeSinceLastTick = 0;
        }

        tickAccumulator += delta;
    }
}