using UnityEngine;

public class Tree : MonoBehaviour, IInteractable, ICollected
{
    public Item item 
    {
        get 
        {
            return new Item(0, "Древесина"); //предмет который мы получим при уничтожении
        } 
    }

    public IAnimatedAction TypeInteraction()
    {
        return new AttackingInteraction(); //взаимодействие с деревом проходит через атаку
    }
    public void Interaction(Player player)
    {
        Destroy(gameObject);
    }

    private void OnDestroy() => Spawner.countSpawnedObjects--;//событие срабатываемое при уничтожении объекта
}