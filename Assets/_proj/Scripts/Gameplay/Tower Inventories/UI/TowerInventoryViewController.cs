using Game.Towers.Configs;

namespace Game.TowerInventories.UI
{
    public class TowerInventoryViewController
    {
        private TowerInventoryView _towerInventoryView;
        private TowerInventoryController _towerInventoryController;
        private TowerConfigService _towerConfigService;

        public TowerInventoryViewController(TowerInventoryController towerInventoryController, TowerInventoryView towerInventoryView, TowerConfigService towerConfigService)
        {
            _towerInventoryView = towerInventoryView;
            _towerInventoryController = towerInventoryController;
            _towerConfigService = towerConfigService;

            _towerInventoryController.SetEnd += OnTowerInventorySlotSet;
            _towerInventoryController.UnsetEnd += OnTowerInventorySlotUnset;
            _towerInventoryController.StackCountIncreased += OnTowerStackCountIncreased;
            _towerInventoryController.StackCountDecreased += OnTowerStackCountDecreased;
        }

        private void OnTowerInventorySlotSet(int slotIndex)
        {
            var inventorySlotCount = _towerInventoryController.GetSlotStackCount(slotIndex);
            var towerConfigId = _towerInventoryController.GetSlotTowerId(slotIndex);

            var towerConfig = _towerConfigService.GetTowerConfig((int)towerConfigId);

            _towerInventoryView.SetName(slotIndex, towerConfig.TowerName);
            _towerInventoryView.SetSprite(slotIndex, towerConfig.TowerIcon);
            _towerInventoryView.SetTowerCount(slotIndex, inventorySlotCount);
        }
        private void OnTowerInventorySlotUnset(int slotIndex)
        {
            _towerInventoryView.Clear(slotIndex);
        }

        private void OnTowerStackCountIncreased(int slotIndex)
        {
            var inventorySlotCount = _towerInventoryController.GetSlotStackCount(slotIndex);
            _towerInventoryView.SetTowerCount(slotIndex, inventorySlotCount);
        }
        private void OnTowerStackCountDecreased(int slotIndex)
        {
            var inventorySlotCount = _towerInventoryController.GetSlotStackCount(slotIndex);
            _towerInventoryView.SetTowerCount(slotIndex, inventorySlotCount);
        }
    }
}