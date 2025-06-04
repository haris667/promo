using Items;
using UnityEngine;
using Zenject;

namespace Installers 
{
    
    public class InteractionInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<InteractionController>().
                AsSingle();
        }
    }
}