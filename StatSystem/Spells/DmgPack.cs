using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    /// <summary>
    /// Контейнер для урона
    /// </summary>
    public class DmgPack
    {
        public DmgType Type;
        public float Value;
        /// <summary>
        /// Это поле может быть null если источник урона не персонаж
        /// </summary>
        public IEntity Source;

        public DmgPack(DmgType type, float value, IEntity source)
        {
            Type = type;
            Value = value;
            Source = source;
        }
    }
}
