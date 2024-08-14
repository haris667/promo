using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Пояснялка
/// Допустим базовое значение стата ХП == 100
/// После модификатора от предмета ПХ становится == 200
/// Но Максимально допустимое значение ХП == 150
/// Поэтому текущее значение ХП == 150
/// </summary>
public class Stat
{
    private float _current;

    [JsonProperty("type")]
    public StatType Type { get; set; }
    /// <summary>
    /// Базовое значение характеристики
    /// </summary>
    [JsonProperty("standard_value")]
    public float Standard { get; set; }
    /// <summary>
    /// Значение характеристики после манипуляций с модификаторами
    /// </summary>
    [JsonProperty("extended_value")]
    public float Extended { get; set; }
    /// <summary>
    /// Текущее значение характеристики
    /// </summary>
    [JsonProperty("current_value")]
    public float Current
    { 
        get => _current; 
        set => _current = Math.Clamp(value, Min, Max);
    }

// Максимальное и минимальное допустимое значение характеристики
[JsonProperty("min_value")]
    public float Min { get; set; }
    [JsonProperty("max_value")]
    public float Max { get; set; }

    public Stat(StatType type, float standard, float extended, float current, float min, float max )
    {
        Min = min;
        Max = max;

        Type = type;
        Standard = standard;
        Extended = extended;
        Current = current;
    }
}

