using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using System.Threading.Tasks;

public class BehaviourTest : MonoBehaviour
{

    public U_Unit entity;
    public List<BehaviourType> typeEffects = new List<BehaviourType>();
    public List<float> duration = new List<float>();
    public List<float> allDamage = new List<float>(); //каждую 1 сек
    public float startDelay = 2;

    // Start is called before the first frame update
    private async void Start()
    {
        startDelay *= 1000;
        await Task.Delay((int)startDelay);

        float count = typeEffects.Count;
        for (int i = 0; i < count; i++)
            entity.AddEffect(new EffectPack(typeEffects[i], duration[i], null));
        AddDamage();
        AddItems();
        AddChange();
    }
    async void AddDamage()
    {
        await Task.Run(() => Task.Delay(300));

        for (int i = 0; i < allDamage.Count; i++)
        {
            await Task.Run(() => Task.Delay(200));
            entity.AddDamage(new DmgPack(DmgType.Pure, allDamage[i], null));
        }
    }
    async void AddChange()
    {
        await Task.Run(() => Task.Delay(300));
        entity.AddChange(new StatChangePack(StatType.HP, 15, 10));
    }
    async void AddItems()
    {
        Dictionary<StatType, float> items = new Dictionary<StatType, float>();
        items.Add(StatType.HP, 27.5f);
        entity.AddItem(new TestItemF(0, 0, "s", "a", items));
    }

    // Update is called once per frame
    /*void Update()
    {
        Debug.Log($"ActiveEffects:{entity.battleCalculator.activeEffects.Count} ");
        Debug.Log($"HP: {entity.Stats[(int)StatType.HP].Current}");
    }*/
}
