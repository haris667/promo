using Inventory;
using System;
using System.Collections.Generic;
using Zenject;

namespace Items
{
    public interface IInteractionHandler
    {
        public void HandleInteraction(InteractionContext context);
    }

    /// <summary>
    /// Индекс элемента перечисления = реализации взаимодействия.
    /// В InteractionRepository хранятся в том же порядке, что и в enum.
    /// Можно и без этого, конечно, но лучше реализации не придумал. (атрибуты и рефлексия сверхнагрузка, а свич кейсы фе)
    /// non actual
    /// </summary>
    public enum EInteractionType
    {
        Use, 
        AddToInventory,
        RemoveFromInventory,
        Null,
        Destroy


        //особые квестовые взимодействия ниже ->
    }

    /// <summary>
    /// Для прокидывания инфы при взаимодействии.
    /// В перспективе может сильно вырасти, но пока можно не париться
    /// </summary>
    public class InteractionContext
    {
        public ItemConfig itemConfig { get; }
        public InventoryPresenter inventory { get; }
        public ItemView itemView;

        public InteractionContext(ItemConfig config, InventoryPresenter inventory, ItemView itemView)
        {
            itemConfig = config;
            this.inventory = inventory;
            this.itemView = itemView;
        }
    }

    /// <summary>
    /// Очень плохой класс, тк контрится он обычным свич кейсом. Но я слишком долго думал над этим, поэтому оставлю до лучших времен, когда я буду посильнее
    /// и смогу сделать норм ультимативное решение для связи enum - реализация интерфейса без свичей, рефлексии и такой шняги снизу.
    /// </summary>
    public class InteractionRepository
    {
        private static readonly Dictionary<EInteractionType, IInteractionHandler> _handlers =
            new Dictionary<EInteractionType, IInteractionHandler>();

        public static void Register(EInteractionType type, IInteractionHandler implement)
        {
            _handlers[type] = implement;
        }

        public static IInteractionHandler CreateHandler(EInteractionType type)
        {
            if (_handlers.TryGetValue(type, out var implement))
            {
                return implement;
            }
            throw new ArgumentException($"No handler registered for {type}");
        }

        static InteractionRepository()
        {
            Register(EInteractionType.Use, new UseInteraction());
            Register(EInteractionType.AddToInventory, new AddToInventoryInteraction());
            Register(EInteractionType.RemoveFromInventory, new RemoveFromInventoryInteraction());
            Register(EInteractionType.Destroy, new DestroyInteraction());
        }
    }
}