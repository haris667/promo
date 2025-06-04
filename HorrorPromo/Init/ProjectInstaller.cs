using UnityEngine;
using Zenject;

namespace Init 
{
    public class ProjectInstaller : MonoInstaller
    {
        [Header("All your installers:")]
        [SerializeField]
        private DiContainer _container;

        public void Init()
        {
            _container = ProjectContext.Instance.Container;
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// Этим методом мы сами контролируем время старта инсталлов
        /// </summary>
        public void Install()
        {

        }
    }
}