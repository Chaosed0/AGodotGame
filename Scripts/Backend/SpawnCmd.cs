using System.Threading.Tasks;
using EdTestGame.Scripts.Nodes;
using Godot;

namespace EdTestGame.Backend;

public static class SpawnCmd
{
    public async static Task Spawn(Creature creature, Vector2I location)
    {
        MapCreature mapCreature = new MapCreature();
        mapCreature.creature = creature;
        mapCreature.position = location;
        
        GameManager.Instance.session.map.creatures.Add(mapCreature);
        await RootNode.Instance.map.SpawnCreature(creature, location);
    }
}