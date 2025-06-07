using UnityEngine;
using Random = UnityEngine.Random;

namespace Horror {
    
    public class RandomGeneratorHelper
    {
        /// <summary>
        /// Генерирует случайное число в диапазоне [-n, n] с заданной точностью
        /// </summary>
        /// <param name="n">Граница диапазона (положительное число)</param>
        /// <param name="j">Точность (количество знаков после запятой)</param>
        /// <returns>Случайное число с указанной точностью</returns>
        public static float GenerateRandomInRange(float n, int j)
        {
            if (n <= 0) n = 1f;
    
            if (j < 0) j = 0;
    
            float randomValue = Random.value;
    
            float rangedValue = (randomValue * 2f * n) - n;
    
            float multiplier = Mathf.Pow(10f, j);
            float roundedValue = Mathf.Round(rangedValue * multiplier) / multiplier;
    
            return roundedValue;
        }
    }
    
}
