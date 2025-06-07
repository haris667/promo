using Items;
using Quests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Horror
{
    public class UsedItemsEventCreator : MonoBehaviour
    {
        [SerializeField]
        private List<ItemView> items;

        public void Create(UsedItemsEventSettings settings)
        {
            StartCoroutine(ApplyUsedItemsEffectRoutine(settings));
        }

        private IEnumerator ApplyUsedItemsEffectRoutine(UsedItemsEventSettings settings)
        {
            if (settings.delay > 0)
                yield return new WaitForSeconds(settings.delay);

            foreach (var item in items)
            {
                IUseItem usableItem = item as IUseItem;
                if (usableItem != null)
                {
                    usableItem.Use(false);
                    continue;
                }
            }
                
        }
    }
}
