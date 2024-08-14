using System;
using System.Linq;

namespace Game
{
    public class StatCalculator
    {
        private IEntity _entity;

        public void Init(IEntity entity)
        {
            _entity = entity;
        }
        /// <summary>
        /// Прибавка к статистике
        /// Дабы удобно считать прибавку к статистике через айтемы
        /// </summary>
        public void CalculateAdd(Item item)
        {
            var stats = _entity.Stats.Where(stat => item.StatsChange.ContainsKey(stat.Type));
            
            foreach (var stat in stats)
            {
                switch (stat.Type)
                {
                    case StatType.HP:
                        PrimaryCountHP(stat, item.StatsChange[stat.Type]);
                        break;
                    default:
                        BaseCountStat(stat, item.StatsChange[stat.Type]);
                        break;
                }
            }

            item.Equip();
        }
        /// <summary>
        /// Прибавка к статистике
        /// Дабы удобно считать отнимание у статистики через айтемы
        /// </summary>
        public void CalculateRemove(Item item)
        {
            var stats = _entity.Stats.Where(stat => item.StatsChange.ContainsKey(stat.Type));

            foreach (var stat in stats)
            {
                switch (stat.Type)
                {
                    case StatType.HP:
                        PrimaryCountHP(stat, -item.StatsChange[stat.Type]);
                        break;
                    default:
                        BaseCountStat(stat, -item.StatsChange[stat.Type]);
                        break;
                }
            }

            item.Unequip();
        }

        /// <summary>
        /// Для уникального подсчета статы. Можно дописать подсчет других статов и расширить методы выше
        /// </summary>
        private void PrimaryCountHP(Stat stat, float value)
        {
            stat.Extended += value / 4;
            stat.Current += value / 4;
        }
        /// <summary>
        /// Для стандартного подсчета статистики. 
        /// Так считаются все статы, подсчет которых не уникальный
        /// </summary>
        private void BaseCountStat(Stat stat, float value)
        {
            stat.Extended += value;
            stat.Current += value;
        }
    }
}
