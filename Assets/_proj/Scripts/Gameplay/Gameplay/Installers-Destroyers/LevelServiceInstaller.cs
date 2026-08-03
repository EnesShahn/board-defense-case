using ESF.Core.Logging;
using ESF.Core.Services;
using Game.Bases;
using Game.Cells;
using Game.Enemies.Configs;
using Game.TowerInventories;
using Game.TowerInventories.UI;
using Game.Towers.Configs;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class LevelServiceInstaller : MonoBehaviour
{
    [SerializeField] private TowerConfigCollection _towerConfigCollection;
    [SerializeField] private EnemyConfigCollection _enemyConfigCollection;
    [SerializeField] private CellManager _cellManager;
    [SerializeField] private BaseRoot _baseRoot;

    [SerializeField] private TowerInventoryView _towerInventoryView;

    private void Awake()
    {
        ELogger.Log<LevelServiceInstaller>("Start Services creation - Awake");

        Service.Register<CellManager>(_cellManager);
        Service.Register<BaseRoot>(_baseRoot);

        var enemyConfigService = new EnemyConfigService(_enemyConfigCollection.EnemyConfigs);
        enemyConfigService.InitializeConfigsAndPools();
        Service.Register<EnemyConfigService>(enemyConfigService);

        var towerConfigService = new TowerConfigService(_towerConfigCollection.TowerConfigs);
        towerConfigService.InitializeConfigsAndPools();
        Service.Register<TowerConfigService>(towerConfigService);

        var towerInventoryController = new TowerInventoryController();
        Service.Register<TowerInventoryController>(towerInventoryController);

        var towerInventoryViewController = new TowerInventoryViewController(towerInventoryController, _towerInventoryView, towerConfigService);
        Service.Register<TowerInventoryViewController>(towerInventoryViewController);


        ELogger.Log<LevelServiceInstaller>("End Services creation - Awake");
    }
}