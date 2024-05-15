using System.Threading.Tasks;
using EdTestGame.Scripts.Nodes;
using Godot;

namespace EdTestGame.Backend;

public static class MoveCmd
{
    public static async Task Move(Creature creature, Vector2I offset)
    {
        Map map = GameManager.Instance.session.map;
        MoveCreatureInfo info = map.MoveCreature(creature, offset);

        if (info.result == MoveCreatureResult.Success)
        {
            await GameNode.Instance.map.MoveCreatureTo(creature, info.newLocation);
        }
        else if (info.result == MoveCreatureResult.Blocked)
        {
            if (info.blockingCreature.creature.faction != creature.faction)
            {
                await DamageCmd.Damage(creature, info.blockingCreature.creature, 1);
            }
        }
    }
}