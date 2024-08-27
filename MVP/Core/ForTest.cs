using Core.Services;
using Core.Commands;
using UnityEngine;

namespace Core
{
    public class ForTest : MonoBehaviour
    {
        public ExampleDistanceCalculateService service;
        void Start()
        {
            service = new ExampleDistanceCalculateService(new ExamplePositionCommand(Vector3.zero));
            Debug.Log(service.Execute(Vector3.one));
        }
    }
}
