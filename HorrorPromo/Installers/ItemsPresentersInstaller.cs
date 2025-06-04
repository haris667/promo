using Items;
using Zenject;

namespace Installers
{
    public class ItemPresentersInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ItemPresenter>().
                AsTransient().
                WithArguments(new ItemModel());
        }
    }
}