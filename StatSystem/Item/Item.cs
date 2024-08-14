using Newtonsoft.Json;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace Game
{
    //Item �������������� ������ �� ����� ������
    public abstract class Item
    {
        [JsonProperty("global_id")]
        public long GlobalId { get; set; }
        public int GameId { get; set; }
        [JsonProperty("name")]
        public string Name { get; }
        [JsonProperty("description")]
        public string Description { get; }
        [JsonProperty("stats_change")]
        public Dictionary<StatType, float> StatsChange { get; }

        public Item(long globalId, int gameId, string name, string description, Dictionary<StatType, float> statsChange)
        {
            GlobalId = globalId;
            GameId = gameId;
            Name = name;
            Description = description;
            StatsChange = statsChange;
        }

        /// <summary>
        /// ����� ������� ����������
        /// ��� ����� ������ �� ���� 
        /// ��� ����� ���� �������� �� ����� �� ������� �� ������� ��������� �������
        /// </summary>
        public virtual void Equip() { }
        /// <summary>
        /// ����� ������� ���������
        /// ��� ����� ������ �� ���� 
        /// ��� ����� ���� ������� �� ����� �� ������� �� ������� ��������� �������
        /// </summary>
        public virtual void Unequip() { }
    }
}