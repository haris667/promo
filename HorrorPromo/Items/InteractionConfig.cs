using UnityEngine;

namespace Items
{
    [CreateAssetMenu(fileName = "NewInteractionConfig", menuName = "Configs/interactionConfig")]
    [System.Serializable]
    public class InteractionConfig : ScriptableObject
    {
        public float interactionDistance = 3f;
        public LayerMask interactionLayer;
    }
}