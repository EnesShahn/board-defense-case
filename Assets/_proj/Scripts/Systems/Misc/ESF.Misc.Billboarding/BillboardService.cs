using System.Collections.Generic;
using ESF.Core.Services;
using ESF.Core.UpdateScheduler;
using UnityEngine;

namespace ESF.Misc.Billboarding
{
    public class BillboardService
    {
        private UpdateService _updateService;
        private Transform _theTarget;

        private HashSet<BillboardController> _billboardControllers;

        public Transform TheTarget => _theTarget;

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
        
        public void SetTarget(Transform target)
        {
            _theTarget = target;
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
            if (_theTarget == null)
                return;

            foreach (var billboardController in _billboardControllers)
            {
                billboardController.BillboardUpdate();
            }
        }
    }
}