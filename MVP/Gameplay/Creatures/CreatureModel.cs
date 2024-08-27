using Core.MVP;
using Gameplay.Creatures.States;
using Gameplay.Creatures.Stats;
using System.Collections.Generic;

namespace Gameplay.Creatures
{
    public class CreatureModel : AModel
    {
        public string name;
        public string description;
        public Fraction fraction;

        public List<Stat> stats;
        public List<CreatureState> states;
    }

    public enum Fraction
    {
        Human,
        Undead,
        Animals
    }
}
