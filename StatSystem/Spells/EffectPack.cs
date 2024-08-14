using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class EffectPack
    {
        /// <summary>
        /// В какое состояние будет переведена цель
        /// Думаю IsDead лучше не прокидывать :D
        /// </summary>
        public BehaviourType Type;
        /// <summary>
        /// В секундах
        /// </summary>
        public float Duration;
        /// <summary>
        /// Остаток времени до конца действия. Можно использовать для таймера, отнимая Time.deltaTime
        /// Остальные реализации таймеров громоздкие
        /// Так же, можно юзать для отображения остатка эффекта игроку
        /// </summary>
        public float RemainTime; 
        /// <summary>
        /// Это поле может быть null если источник урона не персонаж
        /// </summary>
        public IEntity Source;

        public EffectPack(BehaviourType type, float duration, IEntity source)
        {
            if (type == BehaviourType.IsDead)
                throw new Exception("Не надо так делать");

            Type = type;
            Duration = duration;
            RemainTime = duration;
            Source = source;

        }
    }
}
