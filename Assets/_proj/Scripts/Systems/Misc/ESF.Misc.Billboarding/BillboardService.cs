using System.Collections.Generic;
using ESF.Core.Services;
using ESF.Core.UpdateScheduler;
using UnityEngine;

namespace ESF.Misc.Billboarding
{
    public class BillboardService
    {
        private UpdateService _updateService;
        private Camera _targetCamera;

        private HashSet<BillboardController> _billboardControllers;

        public Camera TargetCamera => _targetCamera;

        public BillboardService(ReadonlyServiceContainer serviceContainer)
        {
            _updateService = serviceContainer.Resolve<UpdateService>();

            _billboardControllers = new();
            _updateService.OnUpdate += OnUpdate;
        }
        public void Deinitialize()
        {
            _updateService.OnUpdate -= OnUpdate;
        }
        
        public void SetTargetCamera(Camera targetCamera)
        {
            _targetCamera = targetCamera;
        }

        public void RegisterBillboard(BillboardController billboardController)
        {
            _billboardControllers.Add(billboardController);
        }
        public void UnregisterBillboard(BillboardController billboardController)
        {
            _billboardControllers.Remove(billboardController);
        }

        private void OnUpdate()
        {
            if (_targetCamera == null)
                return;

            foreach (var billboardController in _billboardControllers)
            {
                billboardController.BillboardUpdate();
            }
        }
    }
}