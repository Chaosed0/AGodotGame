using System;
using System.Collections.Generic;
using Godot;

namespace EdTestGame.Backend;

public class Map
{
    public List<MapCreature> creatures = new List<MapCreature>();

    public MoveCreatureInfo MoveCreature(Creature creature, Vector2I offset)
    {
        for (int i = 0; i < creatures.Count; i++)
        {
            MapCreature candidate = creatures[i];
            
            if (candidate.creature == creature)
            {
                Vector2I newLocation = candidate.position + offset;
                MapCreature blockingCreature = GetCreatureAtLocation(newLocation);

                MoveCreatureInfo info = default;

                if (blockingCreature != null)
                {
                    info.blockingCreature = blockingCreature;
                    info.result = MoveCreatureResult.Blocked;
                    info.newLocation = candidate.position;
                }
                else
                {
                    info.blockingCreature = null;
                    info.result = MoveCreatureResult.Success;
                    info.newLocation = newLocation;
                    
                    candidate.position = newLocation;
                }

                return info;
            }
        }

        throw new ArgumentException("Creature not in map");
    }

    public void RemoveCreature(Creature creature)
    {
        for (int i = 0; i < creatures.Count; i++)
        {
            if (creatures[i].creature == creature)
            {
                creatures.RemoveAt(i);
                i--;
            }
        }
    }

    public MapCreature GetCreatureAtLocation(Vector2I location)
    {
        for (int i = 0; i < creatures.Count; i++)
        {
            if (creatures[i].position == location)
            {
                return creatures[i];
            }
        }

        return null;
    }

    public Vector2I GetLocation(Creature creature)
    {
        for (int i = 0; i < creatures.Count; i++)
        {
            if (creatures[i].creature == creature)
            {
                return creatures[i].position;
            }
        }

        throw new ArgumentException("Creature not in map");
    }
}

public enum MoveCreatureResult
{
    Success,
    Blocked,
}

public struct MoveCreatureInfo
{
    public MoveCreatureResult result;
    public Vector2I newLocation;
    public MapCreature blockingCreature;
}

public class MapCreature
{
    public Vector2I position;
    public Creature creature;
}
    
