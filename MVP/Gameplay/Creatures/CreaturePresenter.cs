using Core.Commands;
using Core.MVP;
using System.Linq;

namespace Gameplay.Creatures
{
    public class CreaturePresenter : APresenter<CreatureModel, CreatureView>
    {
        public CreaturePresenter(CreatureModel model, CreatureView view) : base(model, view) { }

        public void AddStates(CreatureStateCommand command)
        {
            _model.states.AddRange(command.states);
        }

        public void RemoveStates(CreatureStateCommand command)
        {
            _model.states.Except(command.states);
        }
    }
}
