using System.Threading.Tasks;
using EdTestGame.Backend;
using Godot;

namespace EdTestGame.Scripts.Nodes;

public partial class CreatureNode : Node2D
{
    [Export]
    public Sprite2D spriteNode;

    public Creature creature { get; private set; }
    
    private MapNode _map;

    public void Initialize(MapNode map, Creature creature)
    {
        _map = map;
        this.creature = creature;
        
        if (creature.type == CreatureType.Player)
        {
            spriteNode.Texture = GD.Load<Texture2D>("res://Resources/player_tex.tres");
        }
        else if (creature.type == CreatureType.Goblin)
        {
            spriteNode.Texture = GD.Load<Texture2D>("res://Resources/goblin_tex.tres");
        }
    }

    public Task DoSpawnIn()
    {
        Tween tween = CreateTween();
        tween.SetParallel(true);

        float spawnTime = creature.type == CreatureType.Player ? 1.5f : 0.35f;
        
        tween.TweenMethod(Callable.From((Vector2 x) => spriteNode.Position = x), new Vector2(0, -10f), spriteNode.Position, spawnTime);
        tween.TweenMethod(Callable.From((float x) => spriteNode.SelfModulate = spriteNode.SelfModulate with { A = x }), 0f, 1f, spawnTime);
        TaskCompletionSource tcs = new TaskCompletionSource();
        tween.Finished += tcs.SetResult;
        return tcs.Task;
    }

    public Task DoAttack(Creature creature, Vector2I mapLocation, System.Action OnAttackHit)
    {
        Tween tween = CreateTween();
        tween.SetParallel(true);

        Vector2 targetLocation = _map.GetPositionFromMap(mapLocation);
        const float attackTime = 0.1f;
        const float attackReturnTime = 0.2f;
        const float attackRotation = Mathf.Pi * 0.3333f;

        Vector2 attackTarget = spriteNode.Position + (spriteNode.GlobalPosition - targetLocation) / 2f;
        
        tween.TweenMethod(Callable.From((Vector2 x) => spriteNode.Position = x), spriteNode.Position, attackTarget, attackTime);
        tween.Parallel();
        tween.TweenMethod(Callable.From((float x) => spriteNode.Rotation = x), 0f, attackRotation, attackTime);

        tween.TweenCallback(Callable.From(OnAttackHit));
        
        tween.TweenMethod(Callable.From((Vector2 x) => spriteNode.Position = x), attackTarget, Vector2.Zero, attackReturnTime);
        tween.Parallel();
        tween.TweenMethod(Callable.From((float x) => spriteNode.Rotation = x), attackRotation, 0f, attackTime);
        
        TaskCompletionSource tcs = new TaskCompletionSource();
        tween.Finished += tcs.SetResult;
        return tcs.Task;
    }

    public Task DoDie()
    {
        Tween tween = CreateTween();
        tween.SetParallel(true);

        tween.TweenMethod(Callable.From((Vector2 x) => spriteNode.Scale = x), Vector2.One, new Vector2(2f, 0.1f), 0.5f);
        tween.TweenMethod(Callable.From((float x) => spriteNode.SelfModulate = spriteNode.SelfModulate with { A = x }), 1f, 0f, 0.5f);
        
        TaskCompletionSource tcs = new TaskCompletionSource();
        tween.Finished += tcs.SetResult;
        return tcs.Task;
    }

    public Task DoFlinch()
    {
        Tween tween = CreateTween();
        RandomNumberGenerator rng = new RandomNumberGenerator();

        for (int i = 0; i < 10; i++)
        {
            float rotation = rng.RandfRange(-Mathf.Pi, Mathf.Pi);
            tween.TweenProperty(spriteNode, "position", new Vector2(Mathf.Cos(rotation), Mathf.Sin(rotation)), 0.05f)
                .SetTrans(Tween.TransitionType.Spring);
        }

        tween.TweenProperty(spriteNode, "position", Vector2.Zero, 0.05f);

        TaskCompletionSource tcs = new TaskCompletionSource();
        tween.Finished += tcs.SetResult;
        return tcs.Task;
    }
}