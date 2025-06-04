using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public static class DictionaryHelper
    {
        public static Dictionary<string, int> ConvertListsToDictionary(List<string> ids, List<int> amounts)                                               
        {
            if (ids == null || amounts == null)
                return new Dictionary<string, int>();

            if (ids.Count != amounts.Count)
            {
                Debug.LogError($"Mismatched list sizes! " +
                               $"IDs: {ids.Count}, Amounts: {amounts.Count}");
                return new Dictionary<string, int>();
            }

            var result = new Dictionary<string, int>();

            for (int i = 0; i < ids.Count; i++)
            {
                string id = ids[i];
                int amount = amounts[i];

                if (result.ContainsKey(id))
                {
                    result[id] += amount;
                }
                else
                {
                    result.Add(id, amount);
                }
            }

            return result;
        }

    }
}
