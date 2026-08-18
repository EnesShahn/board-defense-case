using UnityEngine;

namespace ESF.Core.UpdateScheduler
{
    [DefaultExecutionOrder(UpdateService.PostUpdateHandlerExecutionOrder)]
    public class PostUpdateHandler : MonoBehaviour
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
            _updateService.PostUpdate();
        }

        private void FixedUpdate()
        {
            if (!_initialized) return;
            _updateService.PostFixedUpdate();
        }

        private void LateUpdate()
        {
            if (!_initialized) return;
            _updateService.PostLateUpdate();
        }
    }
}