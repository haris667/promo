using UnityEngine;

namespace Infrastructure.MVP
{
    //помимо ответственности над отображением, берет на себя ответственность инициализации презентера и модели.
    //все ради скорости разработки. Хоть так и не должно быть, но в рамках атомарного проекта - пойдет
    public abstract class AView : MonoBehaviour
    {
        public virtual void Show() => gameObject.SetActive(true);

        public virtual void Hide() => gameObject.SetActive(false);

        protected abstract void Init();

        protected void Awake() => Init();
    }
}