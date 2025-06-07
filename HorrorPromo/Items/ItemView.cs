using Infrastructure.MVP;
using Installers;
using ModestTree;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace Items
{
    public class ItemView : AView
    {
        [SerializeField]
        protected ItemConfig _config;
        [Inject]
        protected ItemPresenter _presenter;
        [Inject]
        private SignalBus _signalBus;

        //избыточная связь View - Presenter. Для скорости разработки - вьюха берет на себя ответственность инициализации.
        //не горю желанием сверхмного работать с DI контейнерами, поэтому в рамках задачи - будет так
        protected override void Init()
        {
            _presenter.SetView(this);
            _presenter.SetConfigToModel(_config);
        }

        public void Suicide()
        {
            _signalBus.Fire<DestroyItemSignal>(new DestroyItemSignal
            {
                config = _config
            });

            Destroy(gameObject);
        }

        public ItemConfig GetConfig() => _config;
    }
}