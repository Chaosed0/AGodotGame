using EdTestGame.Backend;
using Godot;

namespace EdTestGame.Scripts.Nodes;

public partial class CameraNode : Node2D
{
    private CreatureNode _player;

    public override void _Process(double delta)
    {
        if (_player == null)
        {
            foreach (CreatureNode node in GameNode.Instance.map.nodes)
            {
                if (node.creature.faction == Faction.Player)
                {
                    _player = node;
                }
            }
        }

        if (_player == null)
        {
            return;
        }

        Position = Position.Lerp(_player.Position, (float)delta * 4f);
    }
}