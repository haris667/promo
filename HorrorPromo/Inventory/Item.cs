using Items;

namespace Inventory
{
    [System.Serializable]
    public class Item
    {
        public ItemConfig config; //базовые данные из конфига. Те что имеют состояния тут. (статус батареи фонарика например)

        public Item(ItemConfig config)
        {
            this.config = config;
        }
    }
}


