using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace Game
{
    public class AbilityDataLoader
    {

        /// <summary>
        /// Все стартовые значения закешированы здесь. Может пригодиться при 
        /// получении стартовых способок сущности
        /// </summary>
        public Dictionary<Ability, string> _cachedAbilities = new Dictionary<Ability, string>();


        /// <summary>
        /// Путь до json файла.
        /// Дженерик чтобы подгружать данные производных абилки
        /// </summary>
        /// <param name="configPath"></param>
        public Ability LoadAbilityData<T>(string configPath, Transform transform, Pool pool) where T : Ability
        {
            using (StreamReader r = new StreamReader(configPath))
            {
                string json = r.ReadToEnd();
                WriteAbilityJson(json);
                T ability = JsonConvert.DeserializeObject<T>(json);

                ability.source = transform;
                ability.projectilePool = pool;
                return ability;
            }
        }

        /// <summary>
        /// Для записи уникальных json ability.
        /// </summary>
        private void WriteAbilityJson(string json)
        {
            if (_cachedAbilities.ContainsValue(json))
                return;

            _cachedAbilities.Add(JsonConvert.DeserializeObject<Ability>(json), json);
        }
    }
}