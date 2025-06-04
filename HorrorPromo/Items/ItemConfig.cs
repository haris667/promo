using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    [CreateAssetMenu(fileName = "NewItemConfig", menuName = "Configs/ItemConfig")]
    [System.Serializable]
    public class ItemConfig : ScriptableObject
    {
        public string id;
        public string name;

        public Vector3 scaleForInventory = Vector3.one;
        public Vector3 positionOffsetForInventory = Vector3.zero;
        public Vector3 rotationAxisForInventory = Vector3.forward;

        public GameObject prefab;
        public EInteractionType typeInteraction;
        public AudioClip clipInteraction;

        #region ForInteractions
        //видел реализацию полиморфных конфигов, где при определенном введении данных, появлялись новые поля для ввода
        //в моем случае, было бы удобно, чтобы и при разных типах взаимодействий в конфиге показывалось только нужное для выбранного типа
        //но на такую штучку у меня очень много времени уйдет.
        //в таком варианте это костыль
        [Space(10)]
        [Header("Interaction settings")]
        public string idForRemove;
        public int amountForRemove;

        public int amountForAdd;
        #endregion

    }
}