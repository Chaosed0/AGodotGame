﻿using System.Threading.Tasks;
using EdTestGame.Scripts.Nodes;
using Godot;

namespace EdTestGame.Backend;

public static class DamageCmd
{
    public static async Task Damage(Creature source, Creature target, int damageAmount)
    {
        target.health -= damageAmount;

        Vector2I targetLocation = GameManager.Instance.session.map.GetLocation(target);
        
        if (target.health <= 0)
        {
            GameManager.Instance.session.map.RemoveCreature(target);
        }

        CreatureNode sourceNode = GameNode.Instance.map.GetNodeFor(source);
        CreatureNode targetNode = GameNode.Instance.map.GetNodeFor(target);

        await sourceNode.DoAttack(source, targetLocation, () => OnAttackHit(targetNode));
    }

    private static void OnAttackHit(CreatureNode targetNode)
    {
        if (targetNode.creature.health <= 0)
        {
            targetNode.DoDie();

            if (targetNode.creature.faction == Faction.Player)
            {
                GameNode.Instance.OnPlayerDied();
            }
            else
            {
                GameNode.Instance.OnMonsterDied();
            }
        }
        else
        {
            targetNode.DoFlinch();
        }
    }
}
