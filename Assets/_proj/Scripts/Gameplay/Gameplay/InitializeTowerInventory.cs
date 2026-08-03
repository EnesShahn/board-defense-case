using UnityEngine;
using ESF.Core.Services;
using Game.TowerInventories;
using Game.TowerInventories.UI;

namespace Game.Main
{
    public class InitializeTowerInventory : MonoBehaviour
    {
        [SerializeField] private TowerInventoryView _towerInventoryView;
        [SerializeField] private TowerInventoryConfig _towerInventoryConfig;

        private void Awake()
        {
            var towerInventoryController = Service.Resolve<TowerInventoryController>();

            towerInventoryController.Initialize(_towerInventoryConfig.TowerInventories.Length);
            _towerInventoryView.Initialize(_towerInventoryConfig.TowerInventories.Length);

            foreach (var towerInventory in _towerInventoryConfig.TowerInventories)
                towerInventoryController.QuickAddTower(towerInventory.TowerConfigId, towerInventory.TowerCount);
        }
    }
}