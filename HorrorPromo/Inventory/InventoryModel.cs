using Helpers;
using Infrastructure.MVP;
using System.Collections.Generic;
using System.Linq;

namespace Inventory
{
    public class InventoryModel : AModel
    {
        public List<Item> items;

        public List<Item> uniqueItems
        {
            get => items.
                GroupBy(item => item.config.id).
                Select(group => group.First()).
                ToList();
        }

        public int uniqueItemsCount
        {
            get => items.
                Select(item => item.config.id).
                Distinct().
                Count();
        }

        public void Init(InventoryConfig config)
        {
            //items = config.items; //пока не критично, но в этом случае уже нужно клонировать лист, дабы не зависеть от конфига
            //items = DeepCloneHelper.Clone<List<Item>>(config.items);
            items = DeepCloneHelper.Clone(config.items);
        }

        public int GetCountItemsById(string id)
        {
            return items.
                Where(item => item.config.id == id).
                Count();
        }

        public void AddItem(Item item) => items.Add(item);

        public void AddItem(Item item, int amount)
        {
            for(int i = 0; i < amount; i++)
                items.Add(item);
        }

        public void RemoveItem(Item item) => items.Remove(item);

        public void RemoveItemAt(int index) => items.RemoveAt(index);

        public void RemoveItemById(string id, int amount = 1)
        {
            if (amount <= 0) return;

            var indexesToRemove = items
                .Select((item, index) => new { Item = item, Index = index })
                .Where(x => x.Item.config.id == id)
                .Select(x => x.Index)
                .Take(amount)  
                .OrderByDescending(i => i)  
                .ToList();

            foreach (var index in indexesToRemove)
            {
                items.RemoveAt(index);
            }
        }
    }
}


