using Infrastructure.MVP;
using Inventory;
using TMPro;
using UnityEngine;
using Zenject;

namespace Quests
{
    public class QuestView : AView
    {
        [Inject]
        private QuestPresenter _presenter;
        [SerializeField]
        private QuestRepositoryConfig _config;
        [SerializeField]
        private TextMeshProUGUI _taskText;

        protected override void Init()
        {
            _presenter.SetView(this); //такой же инит. Такая же избыточная связь вью и презентера
            _presenter.SetConfigToModel(_config); //в след. попытках подумаю что с этим можно сделать
        }

        public void SetTaskText(string text)
        {
            _taskText.text = "TASK: " + text;
        }
    }
}


