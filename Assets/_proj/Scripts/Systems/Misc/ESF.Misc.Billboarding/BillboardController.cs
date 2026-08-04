using ESF.Core.Services;
using ESF.Utilities.Extensions;
using UnityEngine;

namespace ESF.Misc.Billboarding
{
    public class BillboardController : MonoBehaviour
    {
        [SerializeField] private Transform _centerReference;
        [SerializeField] private Vector3 _relativeToTargetOffset;

        private BillboardService _billboardService;


        private void Awake()
        {
            Service.TryResolve(out _billboardService);
        }
        private void OnEnable()
        {
            if (_billboardService == null)
                return;
            _billboardService.RegisterBillboard(this);
        }
        private void OnDisable()
        {
            if (_billboardService == null)
                return;
            _billboardService.UnregisterBillboard(this);
        }

        public void BillboardUpdate()
        {
            if (_centerReference != null)
            {
                Vector3 lookAtPosition = _billboardService.TargetCamera.transform.position;
                var xzDirection = (transform.position - lookAtPosition.WithY(transform.position.y)).normalized;
                var lookRotation = Quaternion.LookRotation(xzDirection);
                var relativeToTargetOffset = lookRotation * _relativeToTargetOffset;
                transform.position = _centerReference.position + relativeToTargetOffset;
            }

            transform.rotation = _billboardService.TargetCamera.transform.rotation;
        }
    }
}