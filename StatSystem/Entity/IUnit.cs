using Newtonsoft.Json;

public interface IUnit
{
    [JsonProperty("GameId")]
    public int GameId { get; set; }
    [JsonProperty("Name")]
    public string Name { get; set; }
}