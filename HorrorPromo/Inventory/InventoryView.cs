using Infrastructure.MVP;
using TMPro;
using UnityEngine;
using Zenject;

namespace Inventory
{
    public class InventoryView : AView
    {
        public TextMeshProUGUI amountItemText;
        public GameObject emptyTextGO;

        [Inject]
        private InventoryPresenter _presenter;
        [SerializeField]
        private InventoryConfig _config;
        [SerializeField]
        private GameObject _uiRoot;
        [SerializeField]
        private Animator _itemInventoryAnimator;

        protected override void Init()
        {
            _presenter.SetView(this);
            _presenter.SetConfigToModel(_config);
        }

        public override void Show()
        {
            _uiRoot.SetActive(true);
            AnimChangeViewState(false);
        }

        public override void Hide()
        {
            AnimChangeViewState(false);
            _uiRoot.SetActive(false);
        }

        /// <summary>
        /// -1 = left. 1 = right
        /// </summary>
        /// <param name="indexOffset"></param>
        public void SwitchItem(int indexOffset)
        {
            _itemInventoryAnimator.SetTrigger("Switch");
            _presenter.SwitchItem(indexOffset);
        }

        //вообще говоря, по хорошему нужен некий менеджер всех окон. Но такое фундаментальное у меня нет времени писать
        //поэтому костыль до лучших времен :)
        private void AnimChangeViewState(bool state)
        {

        }
    }
}


