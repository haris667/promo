using UnityEngine;

namespace Quests
{
    [System.Serializable]
    public class QuestTask
    {
        public TaskType type;
        public string name;
        public string itemId;
        public string taskId;
        public int requiredAmount;
        public bool hasPriority = false; //у приоритетных задач прогресс считается, только тогда - когда они активны.
                                         //Приоритны потому, потому что они должны быть видны и не пропущены случайно.
                                         //Поэтому лучше не использовать на предметах, которые одноразовы в использовании
                                         //На всякий, ПЕРВАЯ ЗАДАЧА ВСЕГДА ПРИОРИТЕТНА
        [SerializeField]
        private bool _isDisplay = true;
        public bool isDisplay
        {
            get
            {
                if (hasPriority)
                    return true;
                return _isDisplay;
            }
            private set { }
        }
    }

    public enum TaskType
    {
        InsideCoord,
        Destroy,
        Collect,
        Use,
        Remove,
        ButtonClick //например, проверить инвентарь. В целом, для взаимодействием с UI и прочей лабудой которая открывается кнопкой
    }
}


