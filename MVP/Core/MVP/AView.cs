using UnityEngine;

namespace Core.MVP
{
    public abstract class AView : MonoBehaviour
    {
        public void Show() => gameObject.SetActive(true);

        public void Hide() => gameObject.SetActive(false);
    }
}