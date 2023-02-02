using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bag : MonoBehaviour
{
    private List<Item> holder = new List<Item>(); // коллекция предметов

    public Text textTree; //заглушка для текста кол-ва дерева в инвентаре

    public void AddItem(Item item) //добавление предмета
    {
         holder.Add(item);
         textTree.text = CountItemsByID(0).ToString();
    }
    public void RemoveItem(Item item) => holder.Remove(item); //удаление предмета

    public int CountItemsByID(int id)// Количество предметов по айди
    {
        int count = 0;
        for(int i = 0; i < holder.Count; i++) 
            if(holder[i].id == id) count++;

        return count;
    }
    public void RemoveAllItemsByID(int id) //удаление всех предметов по айди
    {
        for(int i = 0; i < holder.Count; i++) 
            if(holder[i].id == id) holder.RemoveAt(id);

        textTree.text = CountItemsByID(0).ToString();// заглушка для обновления текста дерева на сцене
    }
}
