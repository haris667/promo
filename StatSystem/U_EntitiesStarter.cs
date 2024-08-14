using Game;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.IO;
using System.Linq;
using UnityEngine;



/// <summary>
/// Смысл класса в том что это прослойка при старте игры
/// которая инитит статы U_Unit. 
/// </summary>
public class U_EntitiesStarter
{
    public bool awakeLoad = false; //больше для тестов, когда true, инитятся все сущности
    private string jsonBasePath { get => Application.persistentDataPath + @"\BaseEntity.json"; } //Application.persistentDataPath - универсальный путь,
                                                                                                 //есть на всех устройствах
    private string jsonAbilityBasePath { get => Application.persistentDataPath + @"\ThrowingKnives.json"; }
    private static EntityDataLoader entityLoader = new EntityDataLoader();
    private static AbilityDataLoader abilityLoader = new AbilityDataLoader();



    /// <summary>
    /// Для заполнения IEntity из json.
    /// ВАЖНО - json должен быть сериализацией класса Ability. С монобехом U_Unit Не фурычит.
    /// При загрузке сессии нужно цеплять этот метод и перекидывать статы жисоном.
    /// </summary>
    public static void FillEntity(IEntity ientity, string entityJsonPath)
    {
        Entity entity = entityLoader.LoadEntityData(entityJsonPath);

        ientity.GameId = entity.GameId;
        ientity.Name = entity.Name;
        ientity.Stats = entity.Stats;
        ientity.Inventory = entity.Inventory;
        ientity.ActiveItems = entity.ActiveItems;
    }

    public static Ability FillAbility<T>(Ability ability, string jsonPath, Transform source, Pool pool) where T : Ability
    {
        var newAbility = abilityLoader.LoadAbilityData<T>(jsonPath, source, pool);
        FillObjectFields(ability, newAbility);
        return null;
    }

    public static U_CombatController FillCombatController(U_CombatController controller, string jsonPath)
    {
        var newcontroller = entityLoader.LoadAbilitiesData(jsonPath);
        FillObjectFields(controller, newcontroller);
        return controller;
    }

    private static void FillObjectFields(Object dist, Object source)
    {
        var fields = dist.GetType().GetFields();
        var sourceFields = source.GetType().GetFields();

        foreach (var f in fields)
        {
            foreach (var atr in f.CustomAttributes)
            {
                if (atr.GetType() == typeof(JsonProperty))
                {
                    var sourceField = sourceFields.First(i => i.Name == f.Name);
                    f.SetValue(sourceField.GetValue(source), dist);
                }
            }
        }
    }
}