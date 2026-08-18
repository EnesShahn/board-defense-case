using UnityEngine;

namespace ESF.Core.UpdateScheduler
{
    [DefaultExecutionOrder(UpdateService.NormalUpdateHandlerExecutionOrder)]
    public class NormalUpdateHandler : MonoBehaviour
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
            _updateService.Update();
        }

        private void FixedUpdate()
        {
            if (!_initialized) return;
            _updateService.FixedUpdate();
        }

        private void LateUpdate()
        {
            if (!_initialized) return;
            _updateService.LateUpdate();
        }
    }
}