using System;
using ESF.Core.Tags;
using Game.TowerCells;
using Game.Towers.Components;
using Game.Towers.Configs;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace Game.TowerTools
{
    public class TowerDestroyTool
    {
        private TowerConfigService _towerConfigService;

        private int _raycastLayerMask;
        private Camera _camera;

        private bool _toolEnabled;

        private RaycastHit[] _raycastHits;

        public bool ToolEnabled => _toolEnabled;

        public event Action<TowerDestroyedEventArgs> TowerDestroyed;
        public event Action IncorrectInput;

        public TowerDestroyTool(TowerConfigService towerConfigService, int raycastLayerMask, Camera camera)
        {
            _towerConfigService = towerConfigService;
            _raycastLayerMask = raycastLayerMask;
            _camera = camera;
            _raycastHits = new RaycastHit[10];
        }

        public void SetToolState(bool enabled)
        {
            _toolEnabled = enabled;
            if (enabled)
                EnhancedTouchSupport.Enable();
            else
                EnhancedTouchSupport.Disable();
        }

        public void OnUpdate()
        {
            if (!_toolEnabled)
                return;

            if (Touch.activeTouches.Count == 0 && !Mouse.current.leftButton.wasPressedThisFrame)
                return;

            if (EventSystem.current.IsPointerOverGameObject())
                return;

            Ray ray;
            if (Touch.activeTouches.Count == 0)
                ray = _camera.ScreenPointToRay(Mouse.current.position.value);
            else
                ray = _camera.ScreenPointToRay(Touch.activeTouches[0].screenPosition);


            int hitCount = Physics.RaycastNonAlloc(ray, _raycastHits, float.MaxValue, _raycastLayerMask);
            if (hitCount <= 0)
            {
                IncorrectInput?.Invoke();
                return;
            }

            TowerCellTag towerCellTag = null;
            for (int i = 0; i < hitCount; i++)
            {
                var hit = _raycastHits[i];
                // If its Tower Cell then break 
                if (hit.collider.gameObject.TryGetTag<TowerCellTag>(out towerCellTag))
                    break;
            }

            // if not clicked on a tower cell
            if (towerCellTag == null)
            {
                IncorrectInput?.Invoke();
                return;
            }

            // if the Tower Cell is not empty, then destroy tower there
            if (towerCellTag.TowerCellRoot.AttachedTowerRoot != null)
            {
                var towerRoot = towerCellTag.TowerCellRoot.AttachedTowerRoot;
                var towerConfigId = towerRoot.TowerConfig.TowerConfigId;
                var towerCellReference = towerRoot.GetComp<TowerCellReference>();

                bool removed = _towerConfigService.DestroyTower(towerRoot);
                if (removed)
                {
                    towerCellReference.DetachFromTowerRoot();
                    towerCellTag.TowerCellRoot.DetachTowerRoot();
                    TowerDestroyed?.Invoke(new(towerConfigId));
                }
            }
        }

        public class TowerDestroyedEventArgs : EventArgs
        {
            private int _towerConfigId;

            public int TowerConfigId => _towerConfigId;

            public TowerDestroyedEventArgs(int towerConfigId)
            {
                _towerConfigId = towerConfigId;
            }
        }
    }
}