using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public enum StatType
{ 
    HP, 
    MoveSpeedMpl,
    JumpForceMpl,
    Armor,
    MagicArmor,
    RegenHp,
    CritChance,
    Endurance,
    MP,
    AttackSpeedMpl,
    MagicMpl
}

/// <summary>
/// Тип состояния сущности
/// </summary>
public enum BehaviourType
{
    IsStone,
    IsAlive,
    IsDead,
    IsSilence,
    IsStunned,
    IsBusy,
    IsCast,
}

public enum DmgType
{ 
    Physic,
    Magic,
    Fire,
    Pure
}

public enum Tier 
{
    F,
    E,
    D,
    C,
    B,
    A,
    S
}

public enum ItemType
{ 
    Resource,
    Weapon,
}



