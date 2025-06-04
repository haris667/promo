using UnityEngine;

namespace Infrastructure.MVP
{
    //������ ��������������� ��� ������������, ����� �� ���� ��������������� ������������� ���������� � ������.
    //��� ���� �������� ����������. ���� ��� � �� ������ ����, �� � ������ ���������� ������� - ������
    public abstract class AView : MonoBehaviour
    {
        public virtual void Show() => gameObject.SetActive(true);

        public virtual void Hide() => gameObject.SetActive(false);

        protected abstract void Init();

        protected void Awake() => Init();
    }
}