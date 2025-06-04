using System.Collections.Generic;
using UnityEngine;

namespace Quests
{
    [CreateAssetMenu(fileName = "NewQuestConfig", menuName = "Configs/QuestConfig")]
    public class QuestConfig : ScriptableObject
    {
        public string questId;
        public string displayName;
        public List<QuestTask> tasks;
        public string nextQuestId;
        public bool autoActivate = true;
    }
}


