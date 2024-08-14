using Game;
using Newtonsoft.Json;

public interface IEntity : IUnit
{
    [JsonProperty("stats")]
    Stat[] Stats { get; set; }
    /// <summary>
    /// ������� ��������
    /// </summary>
    [JsonProperty("items")]
    Item[] ActiveItems { get; set; }

    [JsonProperty("inventory")]
    Inventory Inventory { get; set; }
}
