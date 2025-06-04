using Infrastructure.MVP;
using ModestTree;
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

        //избыточная связь View - Presenter. Для скорости разработки - вьюха берет на себя ответственность инициализации.
        //не горю желанием сверхмного работать с DI контейнерами, поэтому в рамках задачи - будет так
        protected override void Init()
        {
            _presenter.SetView(this);
            _presenter.SetConfigToModel(_config);
        }

        public void Suicide()
        {
            Destroy(gameObject);
        }

        public ItemConfig GetConfig() => _config;
    }
}