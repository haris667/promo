using Inventory;
using Items;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class InventoryPresentersInstaller : MonoInstaller
    {
        [SerializeField]
        private ItemInventoryView item3d;
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<InventoryPresenter>().
                AsSingle().
                WithArguments(new InventoryModel(), item3d).
                NonLazy();
        }
    }
}
