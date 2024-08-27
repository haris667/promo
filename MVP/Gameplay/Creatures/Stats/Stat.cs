using UnityEngine;

namespace Gameplay.Creatures.Stats
{
    public struct Stat
    {
        public StatType type;
        public string description;

        public float max;
        public float current
        {
            get => _current; 
            set => _current = Mathf.Clamp(value, 0, max);
        }
        private float _current;
    }

    public enum StatType
    {
        Strength,
        Agility,
        Intelligence
    }
}
