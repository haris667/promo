using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Creatures.States
{
    public struct CreatureState
    {
        public CreatureStateType type;
        public float duration;
    }

    public enum CreatureStateType
    {
        Burning,
        Death,
        Stun
    }
}
