using Infrastructure.MVP;
using Installers;
using Items;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Inventory
{
    public class InventoryPresenter : APresenter<InventoryModel, InventoryView>, IDisposable, IInitializable
    {
        [Inject]
        private InputMap _inputMap;
        [Inject]
        private SignalBus _signalBus;

        private bool _isOpen = false;
        private ItemInventoryView _item3dView;

        private int _currentIndex3dView = 0;
        private string _currentId3dView;

        public InventoryPresenter(InventoryModel model, ItemInventoryView item3dView) : base(model) 
        {
            _item3dView = item3dView;
        }
        
        public void Initialize()
        {
            _inputMap.Enable();
            _inputMap.Player.OpenInventory.performed += ChangeViewStateInventory;
            _inputMap.Player.OpenInventory.performed += ChangeViewStateEmptyText;

            if (_model.items.Any())
            {
                _currentId3dView = _model.items[0].config.id; //на старте нулевой элемент показываем
                SetAmountItemsToView(_currentId3dView);       //в ItemInventoryView такая же тема
                //SetRotationAxisTo3dView();
            }                                                 //косячок, это должно как-то контролиться
        }

        public void Dispose()
        {
            _inputMap.Player.OpenInventory.performed -= ChangeViewStateInventory;
            _inputMap.Player.OpenInventory.performed -= ChangeViewStateEmptyText;
        }

        private void ChangeViewStateEmptyText(InputAction.CallbackContext context)
        {
            _view.emptyTextGO.SetActive(!_model.items.Any());
        }

        private void ChangeViewStateEmptyText()
        {
            _view.emptyTextGO.SetActive(!_model.items.Any());
        }

        private void ChangeViewStateInventory(InputAction.CallbackContext context)
        {
            _isOpen = !_isOpen;

            SetCursorState(_isOpen);

            if (_isOpen)
                _view.Show();
            else
                _view.Hide();
        }

        private void SetCursorState(bool state)
        {
            if (state)
                Cursor.lockState = CursorLockMode.None;
            else
                Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = state;
        }

        //немного сложно для восприятия. Я закольцевал слоты для инвентаря. Можно и проще, но будет больше строк
        private Item GetUniqueNearestView(int indexOffset)
        {
            if (_model.uniqueItemsCount == 0)
                return null;

            int newIndex = (_currentIndex3dView + indexOffset) % _model.uniqueItemsCount;
            _currentIndex3dView = newIndex < 0 ? newIndex + _model.uniqueItemsCount : newIndex;

            return _model.uniqueItems[_currentIndex3dView];
        }

        /// <summary>
        ///  -1 = left. 1 = right
        /// </summary>
        /// <param name="indexOffset"></param>
        public void SwitchItem(int indexOffset)
        {
            if (!_model.items.Any())
                return;

            indexOffset = Mathf.Clamp(indexOffset, -1, 1); //protection from genetic freaks

            Item newItem = GetUniqueNearestView(indexOffset);

            _currentId3dView = newItem.config.id;
            SetAmountItemsToView();

            _item3dView.SetNewConfig(newItem.config);
            _item3dView.UpdateModel(newItem.config.prefab, newItem.config); //потом убрать
        }

        private void SetAmountItemsToView(string id)
        {
            _view.amountItemText.text = _model.GetCountItemsById(id).ToString();
        }

        private void SetAmountItemsToView()
        {
            _view.amountItemText.text = _model.GetCountItemsById(_currentId3dView).ToString();
        }

        public override void SetConfigToModel(ScriptableObject config) //конфиг можно будет потом в дженерик засунуть и просто переопределять
        {                                                              //и оставлять типа base.SetConfigToModel<YourTypeSO>(SO config)
            if (config is InventoryConfig inventoryConfig)
                _model.Init(inventoryConfig);
            else
                Debug.LogError("Wrong config type!");
        }

        public void AddItem(Item item, int amount = 1)
        {
            if (!_model.uniqueItems.Any())
            {
                _item3dView.UpdateModel(item.config.prefab, item.config);
                _currentId3dView = item.config.id;
            }
                
            _model.AddItem(item, amount);
            SetAmountItemsToView();

            _signalBus.Fire(new ItemAddedToInventorySignal
            {
                item = item.config,
                amount = amount
            });
        }

        public void RemoveItem(string id, int amount = 1) //id/amount
        {
            if (_model.GetCountItemsById(id) == 0)
                return;

            _model.RemoveItemById(id, amount);
            SetAmountItemsToView();
            TryHandleInventoryAfterItemRemoval(id);

            _signalBus.Fire(new ItemRemovedToInventorySignal
            {
                id = id,
                amount = amount
            });
        }

        private void TryHandleInventoryAfterItemRemoval(string id)
        {
            if (_model.GetCountItemsById(id) != 0)
                return;
            else if (_model.uniqueItemsCount > 1)
                SwitchItem(1); //magic number for you
            else
            {
                ChangeViewStateEmptyText();
                _item3dView.ClearModel();
            }
        }

    }
}


