using Godot;
using System;

namespace EdTestGame.Backend;

public class GameManager : IDisposable
{
    public static GameManager Instance = null;

    public static GameManager Create()
    {
        if (Instance != null)
        {
            GD.PrintErr($"More than one instance of GameManager created!");
            return null;
        }

        Instance = new GameManager();
        return Instance;
    }

    public GameSession session = new();

    public void Dispose()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
