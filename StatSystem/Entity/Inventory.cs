using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    /// <summary>
    /// Экземпляр есть у каждой сущности
    /// </summary>
    public class Inventory
    {
        /// <summary>
        /// Предметы в инвентаре
        /// </summary>
        [JsonProperty("items")]
        public Item[] Items { get; set; }
    }
}
