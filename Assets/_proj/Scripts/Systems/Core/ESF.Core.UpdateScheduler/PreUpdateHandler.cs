using UnityEngine;

namespace ESF.Core.UpdateScheduler
{
    [DefaultExecutionOrder(UpdateService.PreUpdateHandlerExecutionOrder)]
    public class PreUpdateHandler : MonoBehaviour
    {
        private bool _initialized;
        private UpdateService _updateService;

        public void Init(UpdateService updateService)
        {
            _updateService = updateService;
            _initialized = true;
        }

        private void Update()
        {
            if (!_initialized) return;
            _updateService.PreUpdate();
        }

        private void FixedUpdate()
        {
            if (!_initialized) return;
            _updateService.PreFixedUpdate();
        }

        private void LateUpdate()
        {
            if (!_initialized) return;
            _updateService.PreLateUpdate();
        }
    }
}