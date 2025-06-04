using Infrastructure.MVP;
using UnityEngine;

namespace Items
{
    public class ItemModel : AModel
    {
        public GameObject prefab { get; protected set; }
        public EInteractionType typeInteraction { get; protected set; }

        public void Init(ItemConfig config)
        {
            id = config.id;
            prefab = config.prefab;
            typeInteraction = config.typeInteraction;
        }
    }
}