using Game;
using Newtonsoft.Json;
using System;
using System.Linq;
using UnityEngine;
using Zenject;

public class U_Unit : MonoBehaviour, IEntity
{
    public int GameId { get; set; }
    public string Name { get; set; }
    public Stat[] Stats { get; set; }
    public Item[] ActiveItems { get; set; }
    public Inventory Inventory { get; set; }

    public U_InputController input;
    public U_BattleCalculator battleCalculator;
    public U_BehaviourSystem _behaviourSystem;

    private StatCalculator _statCalculator = new StatCalculator();

    private void Awake()
    {
        ActiveItems = new Item[6];
        //InitStats();
        battleCalculator.Init(this);
        _statCalculator.Init(this);

        _behaviourSystem = GetComponent<U_BehaviourSystem>();
        _behaviourSystem.Init(input, battleCalculator);
    }

    public void AddItem(Item item) => _statCalculator.CalculateAdd(item);
    
    public void RemoveItem(Item item) => _statCalculator.CalculateRemove(item);

    public void AddDamage(DmgPack pack) => battleCalculator.SetDmg(pack);

    public void AddEffect(EffectPack pack) => battleCalculator.SetEffect(pack);

    public void AddChange(StatChangePack pack) => battleCalculator.SetChange(pack);
}