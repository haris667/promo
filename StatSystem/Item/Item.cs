using Newtonsoft.Json;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace Game
{
    //Item отредактирован только на время тестов
    public abstract class Item
    {
        [JsonProperty("global_id")]
        public long GlobalId { get; set; }
        public int GameId { get; set; }
        [JsonProperty("name")]
        public string Name { get; }
        [JsonProperty("description")]
        public string Description { get; }
        [JsonProperty("stats_change")]
        public Dictionary<StatType, float> StatsChange { get; }

        public Item(long globalId, int gameId, string name, string description, Dictionary<StatType, float> statsChange)
        {
            GlobalId = globalId;
            GameId = gameId;
            Name = name;
            Description = description;
            StatsChange = statsChange;
        }

        /// <summary>
        /// Когда предмет надевается
        /// Тут может ничего не быть 
        /// Тут может быть подписка на какие то события на которые реагирует предмет
        /// </summary>
        public virtual void Equip() { }
        /// <summary>
        /// Когда предмет снимается
        /// Тут может ничего не быть 
        /// Тут может быть отписка от каких то событий на которые реагирует предмет
        /// </summary>
        public virtual void Unequip() { }
    }
}