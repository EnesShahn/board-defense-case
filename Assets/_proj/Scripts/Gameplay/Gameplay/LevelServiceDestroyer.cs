using ESF.Core.Logging;
using ESF.Core.Services;
using Game.Bases;
using Game.Cells;
using Game.Enemies.Configs;
using Game.TowerInventories;
using Game.TowerInventories.UI;
using Game.Towers.Configs;
using UnityEngine;

namespace Game.Main
{
    [DefaultExecutionOrder(10)]
    public class LevelServiceDestroyer : MonoBehaviour
    {
        private void OnDestroy()
        {
            ELogger.Log<LevelServiceDestroyer>("Start Services destruction");

            Service.Unregister<CellManager>();
            Service.Unregister<BaseRoot>();

            var enemyConfigService = Service.Resolve<EnemyConfigService>();
            enemyConfigService.Dispose();
            Service.Unregister<EnemyConfigService>();

            var towerConfigService = Service.Resolve<TowerConfigService>();
            towerConfigService.Dispose();
            Service.Unregister<TowerConfigService>();

            Service.Unregister<TowerInventoryController>();

            Service.Unregister<TowerInventoryViewController>();

            ELogger.Log<LevelServiceDestroyer>("End Services destruction");
        }
    }
}