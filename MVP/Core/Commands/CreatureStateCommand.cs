using Gameplay.Creatures.States;
using System.Collections.Generic;

namespace Core.Commands 
{
    public class CreatureStateCommand : ICommand
    {
        public List<CreatureState> states;

        public CreatureStateCommand(List<CreatureState> states)
        {
            this.states = states;
        }
    }
}