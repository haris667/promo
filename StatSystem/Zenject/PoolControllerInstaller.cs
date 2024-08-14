using UnityEngine;
using Zenject;

public class PoolControllerInstaller : MonoInstaller
{
    public PoolController container;
    public override void InstallBindings()
    {
        Container.Bind<PoolController>().FromInstance(container).AsSingle().NonLazy();
        Container.QueueForInject(container);
    }
}