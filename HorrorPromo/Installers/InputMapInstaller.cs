using Zenject;

namespace Installers 
{
    public class InputMapInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<InputMap>().
                AsSingle();
        }
    }
}
