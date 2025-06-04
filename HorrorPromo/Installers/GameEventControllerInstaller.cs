using Horror;
using Quests;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class GameEventControllerInstaller : MonoInstaller
    {
        [SerializeField]
        private EventConfig _config;
        [SerializeField]
        private EventExecuter _executer;
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameEventController>().
                AsSingle().
                WithArguments(_config, _executer).
                NonLazy();
        }
    }
}
