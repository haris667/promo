using Core.Commands;
using UnityEngine;

namespace Core.Services 
{
    public class ExampleDistanceCalculateService : AService<ExamplePositionCommand>
    {
        public ExampleDistanceCalculateService(ExamplePositionCommand command) : base(command) { }
        public override Vector3 Execute(Vector3 vector)
        {
            vector = _command.position - vector;
            return base.Execute(vector);
        }
    }
}