using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// У каждой сущности свой экзепляр этого класса
    /// </summary>
    public class U_BattleCalculator : MonoBehaviour
    {
        private IEntity _entity = null;
        /// <summary>
        /// Действующие на сущность эффекты
        /// </summary>
        public List<EffectPack> activeEffects = new List<EffectPack>();
        public Action<EffectPack> OnEffectEnded;
        /// <summary>
        /// Активные изменения
        /// </summary>
        public List<StatChangePack> statChanges = new List<StatChangePack>();
        /// <summary>
        /// Эффекты которые закончили свое действие
        /// </summary>
        private List<EffectPack> _endedEffects = new List<EffectPack>();
        /// <summary>
        /// Сюда сохранять весь !Полученный! урон
        /// </summary>
        private List<DmgPack> _lastDmg = new List<DmgPack>();
        
        public void Init(IEntity entity)
        {
            _entity = entity;
        }

        /// <summary>
        /// Расчет и нанесение урона обычным способом
        /// </summary>
        /// <param name="dmg"></param>
        public void SetDmg(DmgPack dmg)
        {
            switch(dmg.Type)
            {
                case DmgType.Physic:
                    SetPhysicDmg(dmg);
                    break;
                case DmgType.Magic:
                    SetMagicDmg(dmg);
                    break;
                case DmgType.Fire:
                    SetFireDmg(dmg);
                    break;
                default:
                    SetPureDmg(dmg);
                    break;
            }
            _lastDmg.Add(dmg);  // добавлять после модификаторов!!!       
        }

        /// <summary>
        /// Различные типы урона. В SetDmg() соответственно идет распределение урона по этим методам
        /// Для удобного расширения и изменения формул нанесения урона
        /// </summary>
        private void SetPhysicDmg(DmgPack dmg)
        {
            Stat hp = _entity.Stats[(int)StatType.HP];
            Stat armor = _entity.Stats[(int)StatType.Armor];

            hp.Current -= (dmg.Value * (1 / (armor.Current * 2)));
        }
        private void SetMagicDmg(DmgPack dmg)
        {
            Stat hp = _entity.Stats[(int)StatType.HP];
            Stat magicArmor = _entity.Stats[(int)StatType.MagicArmor];

            hp.Current -= (dmg.Value * (1 / magicArmor.Current));
        }
        private void SetFireDmg(DmgPack dmg)
        {
            Stat hp = _entity.Stats[(int)StatType.HP]; 
            Stat armor = _entity.Stats[(int)StatType.Armor];

            hp.Current -= (dmg.Value * ( 1 / (armor.Current * 2)));
        }
        private void SetPureDmg(DmgPack dmg)
        {
            Stat hp = _entity.Stats[(int)StatType.HP];
            hp.Current -= dmg.Value;
        }
        

        /// <summary>
        /// Переводит персонажа в определенное состояние на время 
        /// Длительность каждого эффекта счиатется отдельно, а не складываются
        /// т.е. на персонаже может быть сразу 3 эффекта оглушения и каждый закончится в разное время
        /// </summary>
        /// <param name="effect"></param>
        public void SetEffect(EffectPack effect)
        {
            activeEffects.Add(effect);
        }

        /// <summary>
        /// Изменяет Extended значение характеристики на время действия
        /// Если это прибавка к здоровью и достигнуто максимальное хп, то после снятия эффекта изменения ХП не происхидит
        /// Если не хватало 5 хп до максимума, а изменение было на 10, то прибавится только 5
        /// и при снятии эффекта так же должно сняться ровно сколько добавилось в момент наложения
        /// </summary>
        /// <param name="pack"></param>
        public void SetChange(StatChangePack pack)
        {
            statChanges.Add(pack);
            Stat stat = _entity.Stats[(int)pack.StatType];
            AddStatValue(stat, pack);
        }

        /// <summary>
        /// Для прибавки статы на некоторый промежуток времени.
        /// Например зелье скорости на 10 секунд
        /// </summary>
        private void AddStatValue(Stat stat, StatChangePack pack)
        {
            if (stat.Extended + pack.StatChange > stat.Max)
            {
                pack.FacticStatChange = (stat.Max - stat.Extended);
                stat.Current += pack.FacticStatChange;
            }
            else
                stat.Current += pack.StatChange;

            stat.Extended += pack.StatChange;

        }

        /// <summary>
        /// Дабы удобно и симпатично отнимать измененную стату для возвращения к исходному состоянию
        /// </summary>
        private void RemoveStatValue(Stat stat, StatChangePack pack)
        {
            if (stat.Extended > stat.Max)
                stat.Current -= pack.FacticStatChange;
            else
                stat.Current -= pack.StatChange;// * (stat.Current / stat.Extended);

            stat.Extended -= pack.StatChange;
            Debug.Log(pack.StatChange + " Прибавляем стока " + pack.StatType);
        }
        
        /// <summary>
        /// Тут считать время работы эффектов
        /// </summary>
        private void FixedUpdate() 
        {
            CountTimeEffect();
            CountTimeStatChanges();
        }
        /// <summary>
        /// Для работы со временем необходим таймер.
        /// Идет просмотр по активным эффектам, по истечению времени идет возврат к стандратным хар-кам
        /// </summary>
        private void CountTimeEffect()
        {
            if(activeEffects.Count > 0)
            {
                for(int indexEffect = 0; indexEffect < activeEffects.Count; indexEffect++)
                {
                    EffectPack effect = activeEffects[indexEffect];
                    effect.RemainTime -= Time.fixedDeltaTime;

                    if (effect.RemainTime < 0)
                    {
                        _endedEffects.Add(activeEffects[indexEffect]);
                        activeEffects.RemoveAt(indexEffect);
                        OnEffectEnded?.Invoke(activeEffects[indexEffect]);
                    }
                }
            }
        }

        /// <summary>
        /// Для работы со временем необходим таймер.
        /// Идет просмотр по активным эффектам, по истечению времени идет возврат к стандратным хар-кам
        /// </summary>
        private void CountTimeStatChanges()
        {
            if(statChanges.Count > 0)
            {
                for (int indexChanges = 0; indexChanges < statChanges.Count; indexChanges++)
                {
                    StatChangePack statChangePack = statChanges[indexChanges];
                    statChangePack.RemainTime -= Time.fixedDeltaTime;

                    if (statChangePack.RemainTime < 0)
                    {
                        Stat stat = _entity.Stats[(int)statChangePack.StatType];
                        statChangePack.RemainTime = statChangePack.Duration; //обнаружил баг, если прокидывать пакет урона,
                                                                             //то RemainTime остается тем же что и был.
                                                                             //что и был.
                                                                             //Решение - возврат к исходному или
                                                                             //каждый раз инитить новый пакет

                        RemoveStatValue(stat, statChangePack);
                        statChanges.RemoveAt(indexChanges);
                    }
                }
            }
        }
    }
}
