using Game;
using UnityEngine;

public class U_Player : MonoBehaviour, IEntity, IPlayer
{
    public int GameId { get; set; }
    public string Name { get; set; }
    public Stat[] Stats { get; set; }
    public Item[] ActiveItems { get; set; }
    public Inventory Inventory { get; set; }

    public int NetId { get; set; }
    public string UserName { get ; set; }
}