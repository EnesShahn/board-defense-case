using ESF.Core.Services;
using Game.TowerInventories;
using Game.TowerInventories.UI;
using Game.Towers.Configs;
using Game.TowerTools;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Levels
{
    public class ToolsController : MonoBehaviour
    {
        [SerializeField] private TowerInventoryView _towerInventoryView;
        [SerializeField] private Button _destroyTowerButton;
        [SerializeField] private GameObject _destroyTowerButtonHighlight;
        [SerializeField] private LayerMask _raycastLayerMask;

        private TowerConfigService _towerConfigService;
        private TowerInventoryController _towerInventoryController;
        private int _currentSelectedTowerInventory = -1;
        private Camera _mainCamera;

        private TowerCreateTool _towerCreateTool;
        private TowerDestroyTool _towerDestroyTool;

        private void Awake()
        {
            _towerConfigService = Service.Resolve<TowerConfigService>();
            _towerInventoryController = Service.Resolve<TowerInventoryController>();
            _mainCamera = Service.Resolve<Camera>();

            _towerInventoryView.InventorySlotButtonClicked += OnInventorySlotButtonClicked;
            _destroyTowerButton.onClick.AddListener(OnDestroyTowerButtonClicked);
            _destroyTowerButtonHighlight.SetActive(false);

            _towerCreateTool = new(_towerConfigService, _towerInventoryController, _raycastLayerMask.value, _mainCamera);
            _towerDestroyTool = new(_towerConfigService, _raycastLayerMask.value, _mainCamera);

            _towerCreateTool.TowerCreated += OnToolTowerCreated;
            _towerCreateTool.IncorrectInput += OnTowerCreateToolIncorrectInput;
            _towerDestroyTool.TowerDestroyed += OnTowerDestroyd;
            _towerDestroyTool.IncorrectInput += OnTowerDestroyToolIncorrectInput;
        }


        private void Update()
        {
            if (_towerCreateTool.ToolEnabled)
                _towerCreateTool.OnUpdate();

            if (_towerDestroyTool.ToolEnabled)
                _towerDestroyTool.OnUpdate();
        }

        private void OnDestroyTowerButtonClicked()
        {
            // disable create tool always
            _currentSelectedTowerInventory = -1;
            _towerCreateTool.SetToolState(false);
            if (_currentSelectedTowerInventory != -1)
                _towerInventoryView.SetHighlight(_currentSelectedTowerInventory, false);

            if (_towerDestroyTool.ToolEnabled) // already enabled, toggle = disable
            {
                _towerDestroyTool.SetToolState(false);
                _destroyTowerButtonHighlight.SetActive(false);
            }
            else
            {
                _towerDestroyTool.SetToolState(true);
                _destroyTowerButtonHighlight.SetActive(true);
            }
        }
        private void OnInventorySlotButtonClicked(int slotIndex)
        {
            if (_towerInventoryController.GetSlotStackCount(slotIndex) == 0) // extra validation
                return;

            // disable destroy tool always
            _destroyTowerButtonHighlight.SetActive(false);
            _towerDestroyTool.SetToolState(false);

            // if create tool already enabled and its the selected tower then disable
            if (_towerCreateTool.ToolEnabled && _currentSelectedTowerInventory == slotIndex)
            {
                _towerCreateTool.SetSelectedTower(null);
                _towerCreateTool.SetToolState(false);
                _towerInventoryView.SetHighlight(_currentSelectedTowerInventory, false);
                _currentSelectedTowerInventory = -1;
            }
            else if (_towerCreateTool.ToolEnabled) // If create tool is enabled but click was on different tower then update selection
            {
                var towerConfigId = _towerInventoryController.GetSlotTowerId(slotIndex);
                var towerConfig = _towerConfigService.GetTowerConfig(towerConfigId);

                _towerInventoryView.SetHighlight(_currentSelectedTowerInventory, false);

                _towerCreateTool.SetSelectedTower(towerConfig);
                _towerInventoryView.SetHighlight(slotIndex, true);
                _currentSelectedTowerInventory = slotIndex;
            }
            else // if tool is not even enabled, select the clicked tower
            {
                var towerConfigId = _towerInventoryController.GetSlotTowerId(slotIndex);
                var towerConfig = _towerConfigService.GetTowerConfig(towerConfigId);

                _towerCreateTool.SetSelectedTower(towerConfig);
                _towerCreateTool.SetToolState(true);
                _towerInventoryView.SetHighlight(slotIndex, true);
                _currentSelectedTowerInventory = slotIndex;
            }
        }

        private void OnToolTowerCreated(TowerCreateTool.TowerCreatedEventArgs args)
        {
            if (_towerInventoryController.GetSlotStackCount(_currentSelectedTowerInventory) == 0) // no more towers
                _towerInventoryView.SetInteractable(_currentSelectedTowerInventory, false);
            _towerInventoryView.SetHighlight(_currentSelectedTowerInventory, false);
            _towerCreateTool.SetToolState(false);
            _currentSelectedTowerInventory = -1;
        }
        private void OnTowerCreateToolIncorrectInput()
        {
            _towerInventoryView.SetHighlight(_currentSelectedTowerInventory, false);
            _towerCreateTool.SetToolState(false);
            _currentSelectedTowerInventory = -1;
        }
        private void OnTowerDestroyd(TowerDestroyTool.TowerDestroyedEventArgs args)
        {
            _destroyTowerButtonHighlight.SetActive(false);
            _towerDestroyTool.SetToolState(false);
        }
        private void OnTowerDestroyToolIncorrectInput()
        {
            _destroyTowerButtonHighlight.SetActive(false);
            _towerDestroyTool.SetToolState(false);
        }
    }
}