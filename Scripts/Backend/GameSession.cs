using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;

namespace EdTestGame.Backend;

public class GameSession
{
    private const int TICK_DIVISION = 4;
    
    public Map map = new Map();

    public Vector2I nextPlayerInput;

    private Creature _player;
    private RandomNumberGenerator _rng = new RandomNumberGenerator();
    private int _tickCounter = 0;
    private int _lastPlayerInputTick;

    public async Task Start()
    {
        _player = new Creature();
        await SpawnCmd.Spawn(_player, new Vector2I(0, 0));
    }

    public async void TryHandleLatePlayerInput()
    {
        if (_tickCounter > _lastPlayerInputTick &&
            (_tickCounter - 1) % TICK_DIVISION < 3)
        {
            await TryHandlePlayerInput();
        }
    }

    private async Task TryHandlePlayerInput()
    {
        if (nextPlayerInput != Vector2I.Zero)
        {
            Vector2I playerInput = nextPlayerInput;
            nextPlayerInput = Vector2I.Zero;
            _lastPlayerInputTick = _tickCounter;
            await MoveCmd.Move(_player, playerInput);
        }
    }

    public async void Tick()
    {
        if (_tickCounter % TICK_DIVISION < 3)
        {
            await TryHandlePlayerInput();
        }
        else
        {
            List<Task> tasks = new List<Task>();
            Vector2I playerLocation = map.GetLocation(_player);
            
            for (int i = 0; i < map.creatures.Count; i++)
            {
                MapCreature creature = map.creatures[i];
                
                if (creature.creature.faction == Faction.Enemy)
                {
                    tasks.Add(MoveCmd.Move(creature.creature, GetCardinalTowards(creature.position, playerLocation)));
                }
            }

            for (int i = 0; i < GetNumToSpawn(); i++)
            {
                Creature creature = new Creature();
                creature.faction = Faction.Enemy;
                creature.health = 1;
                creature.type = CreatureType.Goblin;

                Vector2I playerLoc = map.GetLocation(_player);

                for (int j = 0; j < 10; j++)
                {
                    Vector2I offset = GetRandomInCircle(2, 5);
                    
                    if (map.GetCreatureAtLocation(playerLoc + offset) == null)
                    {
                        tasks.Add(SpawnCmd.Spawn(creature, playerLoc + offset));
                        break;
                    }
                }
            }

            await Task.WhenAll(tasks);
        }

        nextPlayerInput = Vector2I.Zero;
        _tickCounter++;
    }

    private int GetNumToSpawn()
    {
        int monsterTurnCount = _tickCounter / TICK_DIVISION;

        if (monsterTurnCount < 4)
        {
            return monsterTurnCount % 2 == 0 ? 1 : 0;
        }
        else
        {
            return monsterTurnCount / 10 + 1;
        }
    }

    private Vector2I GetRandomInCircle(int minRadius, int maxRadius)
    {
        float angle = _rng.RandfRange(-Mathf.Pi, Mathf.Pi);
        float radius = _rng.RandfRange(minRadius, maxRadius);

        return new Vector2I((int)Mathf.Round(Mathf.Cos(angle) * radius), (int)Mathf.Round(Mathf.Sin(angle) * radius));
    }

    private Vector2I GetCardinalTowards(Vector2I from, Vector2I to)
    {
        int xDiff = Math.Abs(from.X - to.X);
        int yDiff = Math.Abs(from.Y - to.Y);
        
        if (xDiff > yDiff)
        {
            return new Vector2I(Math.Sign(to.X - from.X), 0);
        }
        else if (yDiff > xDiff)
        {
            return new Vector2I(0, Math.Sign(to.Y - from.Y));
        }
        else
        {
            if (_rng.Randf() > 0.5)
            {
                return new Vector2I(Math.Sign(to.X - from.X), 0);
            }
            else
            {
                return new Vector2I(0, Math.Sign(to.Y - from.Y));
            }
        }
    }
}