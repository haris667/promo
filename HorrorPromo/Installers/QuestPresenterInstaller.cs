using Quests;
using Zenject;

namespace Installers 
{
    public class QuestPresenterInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<QuestPresenter>().
                AsSingle().
                WithArguments(new QuestModel()).
                NonLazy();
        }
    }
}
