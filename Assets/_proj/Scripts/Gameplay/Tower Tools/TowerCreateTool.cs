using System;
using ESF.Core.Tags;
using Game.TowerCells;
using Game.TowerInventories;
using Game.Towers.Components;
using Game.Towers.Configs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.TowerTools
{
    public class TowerCreateTool
    {
        private TowerConfigService _towerConfigService;
        private TowerInventoryController _towerInventoryController;

        private TowerConfig _selectedTowerConfig;
        private int _raycastLayerMask;
        private Camera _camera;

        private bool _toolEnabled;

        private RaycastHit[] _raycastHits;

        public bool ToolEnabled => _toolEnabled;

        public event Action<TowerCreatedEventArgs> TowerCreated;
        public event Action IncorrectInput;

        public TowerCreateTool(TowerConfigService towerConfigService, TowerInventoryController towerInventoryController, int raycastLayerMask, Camera camera)
        {
            _towerConfigService = towerConfigService;
            _towerInventoryController = towerInventoryController;
            _raycastLayerMask = raycastLayerMask;
            _camera = camera;
            _raycastHits = new RaycastHit[10];
        }

        public void SetToolState(bool enabled)
        {
            _toolEnabled = enabled;
        }
        public void SetSelectedTower(TowerConfig towerConfig)
        {
            _selectedTowerConfig = towerConfig;
        }

        public void OnUpdate()
        {
            if (!_toolEnabled)
                return;

            // return if no taps/clicks yet
            if (Input.touchCount == 0 && !Mouse.current.leftButton.wasPressedThisFrame)
                return;

            Ray ray;
            if (Input.touchCount == 0)
                ray = _camera.ScreenPointToRay(Mouse.current.position.value);
            else
                ray = _camera.ScreenPointToRay(Input.GetTouch(0).position);

            int hitCount = Physics.RaycastNonAlloc(ray, _raycastHits, float.MaxValue, _raycastLayerMask);
            // not a cell, return
            if (hitCount <= 0)
            {
                IncorrectInput?.Invoke();
                return;
            }

            // Get the Tower Cell tag from the list of hits.
            TowerCellTag towerCellTag = null;
            for (int i = 0; i < hitCount; i++)
            {
                var hit = _raycastHits[i];
                // If its Tower Cell then break 
                if (hit.collider.gameObject.TryGetTag<TowerCellTag>(out towerCellTag))
                    break;
            }

            // base cell yes, tower cell no, so break
            if (towerCellTag == null)
            {
                IncorrectInput?.Invoke();
                return;
            }

            // if the Tower Cell is empty, then place tower there
            if (towerCellTag.TowerCellRoot.AttachedTowerRoot == null)
            {
                var towerInstance = _towerConfigService.CreateTower(_selectedTowerConfig.TowerConfigId);
                towerInstance.gameObject.SetActive(true);
                var towerCellReference = towerInstance.GetComp<TowerCellReference>();
                towerCellReference.AttachToTowerRoot(towerCellTag.TowerCellRoot);
                towerInstance.transform.position = towerCellTag.TowerCellRoot.transform.position;
                towerCellTag.TowerCellRoot.AttachTowerRoot(towerInstance);
                _towerInventoryController.QuickRemoveTower(_selectedTowerConfig.TowerConfigId, 1);

                TowerCreated?.Invoke(new(_selectedTowerConfig.TowerConfigId, towerCellTag.TowerCellRoot.CellRoot.CellIndex));
            }
        }

        public class TowerCreatedEventArgs : EventArgs
        {
            private int _towerConfigId;
            private Vector2Int _placedCellIndex;

            public int TowerConfigId => _towerConfigId;
            public Vector2Int PlacedCellIndex => _placedCellIndex;

            public TowerCreatedEventArgs(int towerConfigId, Vector2Int placedCellIndex)
            {
                _towerConfigId = towerConfigId;
                _placedCellIndex = placedCellIndex;
            }
        }
    }
}