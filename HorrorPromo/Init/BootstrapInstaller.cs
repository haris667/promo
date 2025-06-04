using Zenject;

namespace Init 
{
    public class BootstrapInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            //Тут прокидываются в Bootstrap.cs все необходимое для инита.
            //Как ключи для какого-нибудь firebase, так и классы для ин апов допустим
        }
    }
}