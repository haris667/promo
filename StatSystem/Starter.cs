
using Game;
using Newtonsoft.Json;
using System.IO;
using UnityEngine;

public class Starter : MonoBehaviour
{
    public U_Unit unit;
    public U_Unit unit2;
    public U_CombatController combatHuembatTest;
    public PoolController poolController;

    private void Start()
    {
        U_EntitiesStarter.FillEntity(unit, Application.persistentDataPath + @"\BaseEntity.json");
        U_EntitiesStarter.FillEntity(unit, Application.persistentDataPath + @"\BaseEntity.json");

        U_EntitiesStarter.FillEntity(unit2, Application.persistentDataPath + @"\BaseEntity.json");
        U_EntitiesStarter.FillEntity(unit2, Application.persistentDataPath + @"\BaseEntity.json");
        //ThrowingKnives kn = new ThrowingKnives(0, "knives", "throw nive",
        //1, combatHuembatTest.GetComponent<Transform>(), poolController.GetPool(PoolType.Knives));
        /*string result3 =  JsonConvert.SerializeObject(kn, Formatting.None, new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });*/

        //File.WriteAllText(Application.persistentDataPath + @"\ThrowingKnives.json", result3);

        //combatHuembatTest._additionalAttack = U_EntitiesStarter.FillAbility<ThrowingKnives>(combatHuembatTest._additionalAttack, Application.persistentDataPath + @"/ThrowingKnives.json",
        //  combatHuembatTest.GetComponent<Transform>(), poolController.GetPool(PoolType.Knives));

        /*string str = JsonConvert.SerializeObject(combatHuembatTest, Formatting.None, new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        }); 
        File.WriteAllText(Application.persistentDataPath + "/U_CombatPlayer.json", str);*/

        U_EntitiesStarter.FillCombatController(combatHuembatTest, Application.persistentDataPath + "/U_CombatPlayer.json");

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
