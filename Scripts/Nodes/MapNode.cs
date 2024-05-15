using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EdTestGame.Backend;
using Godot;

namespace EdTestGame.Scripts.Nodes;

public partial class MapNode : Node2D
{
    public const float TILE_SIZE = 16f;

    [Export]
    public PackedScene creatureScene;

    public List<CreatureNode> nodes { get; private set; } = new List<CreatureNode>();

    public Task SpawnCreature(Creature creature, Vector2I mapLoc)
    {
        CreatureNode spawned = creatureScene.Instantiate<CreatureNode>();
        spawned.Initialize(this, creature);
        spawned.Position = (Vector2)mapLoc * TILE_SIZE;
        AddChild(spawned);
        nodes.Add(spawned);
        return spawned.DoSpawnIn();
    }

    public CreatureNode GetNodeFor(Creature creature)
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            CreatureNode node = nodes[i];

            if (node.creature == creature)
            {
                return node;
            }
        }

        throw new ArgumentException($"No creature node for {creature}");
    }

    public Vector2 GetPositionFromMap(Vector2I mapLocation)
    {
        return (Vector2)mapLocation * TILE_SIZE;
    }

    public Task MoveCreatureTo(Creature creature, Vector2I mapLoc)
    {
        CreatureNode node = GetNodeFor(creature);
        Tween tween = CreateTween();
        tween.TweenMethod(Callable.From((Vector2 x) => node.GlobalPosition = x), node.GlobalPosition, GetPositionFromMap(mapLoc), 0.1);
        tween.Parallel();
        TaskCompletionSource tcs = new TaskCompletionSource();
        tween.Finished += tcs.SetResult;
        return tcs.Task;
    }
}