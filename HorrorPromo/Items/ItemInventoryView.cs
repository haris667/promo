using Infrastructure.MVP;
using Inventory;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Items
{
    public class ItemInventoryView : AView
    {
        [SerializeField]
        private RenderTexture _renderTexture;

        [SerializeField]
        private RawImage _previewImage;
        [SerializeField]
        private Camera _previewCamera;

        [SerializeField]
        private Transform _modelContainer;

        
        [SerializeField]
        private ItemConfig _config;
        [SerializeField]
        private InventoryConfig _inventoryConfig;

        private GameObject _spawnedModel;
        private Vector3 _rotationAxis;

        protected override void Init()
        {
            Initialize3DPreview();
        }

        private void Initialize3DPreview()
        {
            if (_previewCamera != null)
                _previewCamera.targetTexture = _renderTexture;
            
            if (_previewImage != null)
                _previewImage.texture = _renderTexture;
            
            if(_inventoryConfig.items.Any())
                Spawn3DModel(_inventoryConfig.items[0].config.prefab, _inventoryConfig.items[0].config);
        }

        public void SetRotationAxis(Vector3 axis)
        {
            _rotationAxis = axis;
        }

        public void SetNewConfig(ItemConfig config)
        {
            _config = config;
        }

        private void Spawn3DModel(GameObject prefab, ItemConfig config)
        {
            if (_config == null || _config.prefab == null) return;

            ClearModel();

            //можно до отдельной фабрики масштабировать, но, на этом этапе смысла не вижу (мне лень:)
            _spawnedModel = Instantiate(prefab, _modelContainer);
            _spawnedModel.layer = _previewCamera.gameObject.layer;

            _spawnedModel.transform.position += config.positionOffsetForInventory;
            _spawnedModel.transform.localScale = config.scaleForInventory;
            _rotationAxis = config.rotationAxisForInventory;
        }

        public void ClearModel()
        {
            if (_spawnedModel != null)
            {
                Destroy(_spawnedModel);
            }
        }

        private void Update()
        {
            RotateModel();
        }

        private void RotateModel()
        {
            if (_spawnedModel != null)
            {
                _spawnedModel.transform.Rotate(_rotationAxis, _inventoryConfig.rotationSpeed * Time.deltaTime);
            }
        }

        public override void Show()
        {
            base.Show();
            if (_previewCamera != null) 
                _previewCamera.gameObject.SetActive(true);
        }

        public override void Hide()
        {
            base.Hide();
            if (_previewCamera != null) 
                _previewCamera.gameObject.SetActive(false);
        }

        public void UpdateModel(GameObject newModelPrefab, ItemConfig config) => Spawn3DModel(newModelPrefab, config);

        /// <summary>
        /// Для ответственных, которые не забывают обновлять конфиг (я забыл)
        /// </summary>
        public void UpdateModel()
        {
            Spawn3DModel(_config.prefab, _config);
        }
    }
}