using UnityEngine;
using Zenject;

public class AimTargetSystemInstaller : MonoInstaller
{
    public U_AimTargetSystem system;
    public override void InstallBindings()
    {
        Container.Bind<U_AimTargetSystem>().FromInstance(system).AsSingle().NonLazy();
        Container.QueueForInject(system);
    }
}