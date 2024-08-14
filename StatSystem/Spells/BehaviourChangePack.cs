using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    /// <summary>
    /// Объект объединяющий в себе данные для временного изменения состояния сущности
    /// </summary>
    public class BehaviourChangePack
    {
        public BehaviourChangePack(BehaviourType behaviourType, float statChange, float duration)
        {
            BehaviourType = behaviourType;
            StatChange = statChange;
            Duration = duration;
        }

        public BehaviourType BehaviourType { get; }
        public float StatChange { get; }
        public float Duration { get; }
    }
}
