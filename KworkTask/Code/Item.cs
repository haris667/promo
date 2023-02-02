[System.Serializable]
public struct Item //структура предмета. Ее айди и название
{
    public int id;
    public string name;

    public Item(int id, string name)
    {
        this.id = id;
        this.name = name;
    }
}
