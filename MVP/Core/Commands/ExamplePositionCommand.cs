using UnityEngine;

namespace Core.Commands
{
    public class ExamplePositionCommand : ICommand
    {
        public Vector3 position;

        public ExamplePositionCommand(Vector3 vector)
        {
            position = vector;
        }
    }
}