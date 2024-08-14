using System;
using Game;

[System.Serializable]
public class Entity : IEntity
{
    public int GameId { get; set; }
    public string Name { get; set; }
    public Stat[] Stats { get; set; }

    public Item[] ActiveItems { get; set; }

    public Inventory Inventory { get; set; }

    public Entity(int gameId, string name, Stat[] stats)
    {
        GameId = gameId;
        Name = name;
        Stats = stats;
    }
}