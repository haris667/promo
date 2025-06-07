using Infrastructure.MVP;
using System.Collections.Generic;

namespace Quests
{
    public class QuestModel : AModel
    {
        public List<QuestConfig> allQuests;
        public readonly Dictionary<string, QuestConfig> questsById = new();
        public float delayBeforeNewQuest;

        public void Init(QuestRepositoryConfig config)
        {
            allQuests = config.quests; //пока не критично, но в этом случае уже нужно клонировать лист, дабы не зависеть от конфига
            delayBeforeNewQuest = config.delayBeforeNewQuest;

            //cоздаем словарь для быстрого поиска по ID
            foreach (var quest in allQuests)
                questsById[quest.questId] = quest;
        }
    }
}


