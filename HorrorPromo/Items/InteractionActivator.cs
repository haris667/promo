using Installers;
using Inventory;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Items
{
    /// <summary>
    /// Просто старт взаимодействия по кнопке активируется отсюда
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class InteractionActivator : MonoBehaviour
    {
        [Inject]
        private InputMap _inputMap;

        [Inject]
        private SignalBus _signalBus;

        [Inject]
        private InteractionController _controller;

        private Camera _camera;

        [SerializeField]
        private InteractionConfig _config;

        private void Awake() => Init();

        private void Init()
        {
            _camera = GetComponent<Camera>();

            StartCoroutine(FireAimTypeHitHit());
        }

        private void OnEnable()
        {
            _inputMap.Player.Interaction.performed += TryInteraction;
            _inputMap.Enable();
        }

        private void OnDisable()
        {
            _inputMap.Player.Interaction.performed -= TryInteraction;
            _inputMap.Disable();
        }

        private void TryInteraction(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Ray ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, _config.interactionDistance, _config.interactionLayer))
                {
                    ItemView itemView = hit.collider.GetComponent<ItemView>();

                    if (itemView != null && itemView.GetConfig().typeInteraction != EInteractionType.Null)
                        _controller.Handle(itemView.GetConfig(), itemView);
                    
                }
            }
        }

        //мои пальцы дрожали когда писали это
        //в силу того, что дедлайн уже сегодня - я бы назвал это вынужденной мерой
        //но всегда конечно можно всрать дедлайн и сделать норм, но эта штука должна остаться атомарной
        //поэтому оставлю как есть
        IEnumerator FireAimTypeHitHit()
        {
            while (true)
            {
                yield return new WaitForSeconds(Time.deltaTime);

                Ray ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, _config.interactionDistance, _config.interactionLayer))
                {
                    Debug.DrawLine(ray.origin, hit.point, Color.red);
                    ItemView itemView = hit.collider.GetComponent<ItemView>();

                    if (itemView != null)
                    {
                        _signalBus.Fire(new AimItemSignal());
                        continue;
                    }
                }
                _signalBus.Fire(new AimVoidSignal());
            }
        }
    }
    /// <summary>
    /// Контролирует взаимодействие.
    /// В будущем, возможно, будет масштабироваться и дрбоиться на разные контроллеры для взаимодействия
    /// </summary>
    public class InteractionController
    {
        [Inject]
        private InventoryPresenter _inventoryPresenter;

        public void Handle(ItemConfig config, ItemView itemView) //думаю этих двух полей и презентера хватит, чтобы обработать, как минимум, добрую половину взаимодействий с предметами
        {
            IInteractionHandler handler = InteractionRepository.CreateHandler(config.typeInteraction);
            handler.HandleInteraction(new InteractionContext(config, _inventoryPresenter, itemView));
        }
    }

    public class UseInteraction : IInteractionHandler
    {
        public void HandleInteraction(InteractionContext context)
        {
            var useItem = context.itemView as IUseItem;

            if (useItem != null)
            {
                useItem.Use();
            }
        }
    }

    public class AddToInventoryInteraction : IInteractionHandler
    {
        public void HandleInteraction(InteractionContext context)
        {
            context.inventory.AddItem(new Item(context.itemConfig), context.itemConfig.amountForAdd);
            context.itemView.Suicide();
        }
    }
    
    public class RemoveFromInventoryInteraction : IInteractionHandler
    {
        public void HandleInteraction(InteractionContext context)
        {
            context.inventory.RemoveItem(context.itemConfig.idForRemove, context.itemConfig.amountForRemove);
        }
    }

    public class DestroyInteraction : IInteractionHandler
    {
        [Inject]
        private SignalBus _signalBus;

        public void HandleInteraction(InteractionContext context)
        {
            _signalBus.Fire<DestroyItemSignal>(new DestroyItemSignal
            {
                config = context.itemConfig
            });

            context.itemView.Suicide(); 
        }
    }
}
