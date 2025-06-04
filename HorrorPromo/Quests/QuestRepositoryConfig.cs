using System.Collections.Generic;
using UnityEngine;

namespace Quests
{
    [CreateAssetMenu(fileName = "NewQuestRepositoryConfig", menuName = "Configs/QuestRepositoryConfig")]
    public class QuestRepositoryConfig : ScriptableObject
    {
        public List<QuestConfig> quests;
    }
}


