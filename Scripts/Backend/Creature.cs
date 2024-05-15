namespace EdTestGame.Backend;

public class Creature
{
    public int health = 5;
    public Faction faction;
    public CreatureType type;
}

public enum Faction
{
    Player,
    Enemy
}

public enum CreatureType
{
    Player,
    Goblin,
}