using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Helpers
{
    /// <summary>
    /// Необходим для создания глубокой копии при переносе данных конфигов в модель.
    /// Путем сериализации и десериализации - создается новый объект с новой ссылкой
    /// и прежними значениями.
    /// На первый взгляд костыльно, но, имхо, это лучшая реализация глубокого копирования что я видел.
    /// </summary>
    public class DeepCloneHelper : MonoBehaviour
    {
        public static T[] Clone<T>(T[] array)
        {
            if (array == null) return null;

            using var memoryStream = new MemoryStream();
            var formatter = new BinaryFormatter();

            // Сериализуем массив в поток
            formatter.Serialize(memoryStream, array);
            memoryStream.Seek(0, SeekOrigin.Begin);

            // Десериализуем из потока в новый массив
            return (T[])formatter.Deserialize(memoryStream);
        }

        public static List<T> Clone<T>(List<T> list)
        {
            if (list == null) return null;

            using var memoryStream = new MemoryStream();
            var formatter = new BinaryFormatter();

            // Сериализуем лист в поток
            formatter.Serialize(memoryStream, list);
            memoryStream.Seek(0, SeekOrigin.Begin);

            // Десериализуем из потока в новый массив
            return (List<T>)formatter.Deserialize(memoryStream);
        }
    }

}
