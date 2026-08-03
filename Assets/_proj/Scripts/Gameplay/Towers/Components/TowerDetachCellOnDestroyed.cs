using Game.Towers.Root;
using UnityEngine;

namespace Game.Towers.Components
{
    public class TowerDetachCellOnDestroyed : MonoBehaviour
    {
        [SerializeField] private TowerRoot _towerRoot;
        [SerializeField] private TowerCellReference _towerCellReference;
        [SerializeField] private TowerDestructionController _towerDestructionController;

        private void Awake()
        {
            _towerDestructionController.TowerDestroyed += OnTowerDestroyed;
        }

        private void OnTowerDestroyed(TowerDestructionController towerDestructionController)
        {
            _towerCellReference.TowerCellAttachedTo.DetachTowerRoot();
            _towerCellReference.DetachFromTowerRoot();
        }
    }
}