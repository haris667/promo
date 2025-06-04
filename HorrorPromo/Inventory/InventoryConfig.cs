using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    [CreateAssetMenu(fileName = "NewInventoryConfig", menuName = "Configs/inventoryConfig")]
    [System.Serializable]
    public class InventoryConfig : ScriptableObject
    {
        public List<Item> items;
 
        public float rotationSpeed = 30f;
    }
}


