using Game;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Windows;

namespace Assets.Scripts.Tests
{
    // поменять пути на Application.persistentDataPath
    // форматирование в норм json как смотреть из вс
    public class EntityTests : MonoBehaviour
    {
        private void Start()
        {
            SerializeTest();
            DeserializeTest();
        }

        private void SerializeTest()
        {
            try
            {
                List<Stat> stats = new List<Stat>();
                Dictionary<StatType, float> statChanges = new Dictionary<StatType, float>();

                foreach (StatType stat in (StatType[])Enum.GetValues(typeof(StatType)))
                {
                    var s = new Stat(stat, 1, 1, 10, 0, 1000000);
                    stats.Add(s);

                    statChanges.Add(stat, 1.2f);
                }

                Entity entity = new Entity(1, "Test", stats.ToArray());

                var testItem1 = new TestItemF(0, 0, "TestItem1", "Это тестовый предмет1", statChanges);
                var testItem2 = new TestItemF(1, 1, "TestItem2", "Это тестовый предмет2", statChanges);
                var testItem3 = new TestItemF(2, 2, "TestItem3", "Это тестовый предмет3", statChanges);

                entity.ActiveItems = new Game.Item[] { testItem1 };

                entity.Inventory = new Inventory();
                entity.Inventory.Items = new Item[] { testItem2, testItem3 };

                string result = JsonConvert.SerializeObject(entity);

                var loc = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                System.IO.File.WriteAllText(Path.Combine(loc, "SerializeTest.json"), result);

                string result2 = JsonConvert.SerializeObject(stats.ToArray());

                System.IO.File.WriteAllText(Path.Combine(loc, "SerializeTestStats.json"), result2);

                string result3 = JsonConvert.SerializeObject(testItem1);

                System.IO.File.WriteAllText(Path.Combine(loc, "SerializeTestItem.json"), result3);

                Debug.Log(Path.Combine(loc, "SerializeTest.json"));

                Debug.Log("SerializeTest Ok");
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
        }

        private void DeserializeTest()
        {
            var loc = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var path = Path.Combine(loc, "SerializeTest.json");

            Entity entity = JsonConvert.DeserializeObject<Entity>(System.IO.File.ReadAllText(path));

            Assert.IsTrue(entity.ActiveItems[0].Name == "TestItem1");

            Assert.IsTrue(entity.Inventory.Items[0].Name == "TestItem2");

            Debug.Log("DeserializeTest Ok");
        }
    }
}
