
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Game
{
    public class EntityDataLoader
    {

        /// <summary>
        /// Все стартовые значения закешированы здесь. Может пригодиться при 
        /// получении стартовых статов сущности
        /// </summary>
        public Dictionary<Entity, string> _cachedEntities = new Dictionary<Entity, string>();


        /// <summary>
        /// Путь до json файла
        /// </summary>
        /// <param name="configPath"></param>
        public Stat[] LoadStatsData(string configPath)
        {
            using (StreamReader r = new StreamReader(configPath))
            {
                string json = r.ReadToEnd();
                WriteEntityJson(json);
                return JsonConvert.DeserializeObject<Stat[]>(json);
            }
        }

        public U_CombatController LoadAbilitiesData(string configPath)
        {
            using (StreamReader r = new StreamReader(configPath))
            {
                string json = r.ReadToEnd();
                WriteEntityJson(json);
                var val = JsonConvert.DeserializeObject<U_CombatController>(json);
                return val;
            }
        }

        /// <summary>
        /// Путь до json файла
        /// </summary>
        /// <param name="configPath"></param>
        public Item LoadItemData(string configPath)
        {
            using (StreamReader r = new StreamReader(configPath))
            {
                string json = r.ReadToEnd();
                WriteEntityJson(json);
                return JsonConvert.DeserializeObject<Item>(json);
            }
        }

        /// <summary>
        /// Путь до json файла
        /// </summary>
        /// <param name="configPath"></param>
        public Entity LoadEntityData(string configPath)
        {
            using (StreamReader r = new StreamReader(configPath))
            {
                string json = r.ReadToEnd();
                WriteEntityJson(json);
                return JsonConvert.DeserializeObject<Entity>(json);
            }
        }

        /// <summary>
        /// Для записи уникальных json entity.
        /// </summary>
        private void WriteEntityJson(string json)
        {
            if (_cachedEntities.ContainsValue(json))
                return;

            _cachedEntities.Add(JsonConvert.DeserializeObject<Entity>(json), json);
        }
    }
}
