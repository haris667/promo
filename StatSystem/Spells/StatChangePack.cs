using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    /// <summary>
    /// Объект объединяющий в себе данные для временного изменения характеристик сущности
    /// </summary>
    public class StatChangePack
    {
        public float Duration;
        public StatType StatType;
        public float StatChange;
        /// <summary>
        /// Фактичесое изменение стата. Актуально в случаях превышения максимума у сущности, 
        /// в иных случаях facticStatChange = statChange
        /// </summary>
        public float FacticStatChange;
        /// <summary>
        /// Остаток времени до конца действия. Можно использовать для таймера, отнимая Time.deltaTime
        /// Остальные реализации таймеров громоздкие
        /// Так же, можно юзать для отображения остатка эффекта игроку
        /// </summary>
        public float RemainTime;
      
        public StatChangePack(StatType statType, float statChange, float duration)
        {
            StatType = statType;
            StatChange = statChange;
            FacticStatChange = statChange;
            Duration = duration;
            RemainTime = duration;
        }
    }
}
