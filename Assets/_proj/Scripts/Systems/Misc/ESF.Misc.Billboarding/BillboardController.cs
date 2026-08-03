using ESF.Core.Services;
using ESF.Utilities.Extensions;
using UnityEngine;

namespace ESF.Misc.Billboarding
{
    public class BillboardController : MonoBehaviour
    {
        [SerializeField] private Transform _centerReference;
        [SerializeField] private Vector3 _relativeToTargetOffset;

        [SerializeField] private bool _freezeXRotation;

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
            Vector3 lookAtPosition = _billboardService.TheTarget.position;
            var xzDirection = (transform.position - lookAtPosition.WithY(transform.position.y)).normalized;


            if (_centerReference != null)
            {
                var lookRotation = Quaternion.LookRotation(xzDirection);
                var relativeToTargetOffset = lookRotation * _relativeToTargetOffset;
                transform.position = _centerReference.position + relativeToTargetOffset;
            }

            if (_freezeXRotation)
                transform.rotation = Quaternion.LookRotation(-xzDirection);
            else
                transform.rotation = Quaternion.LookRotation(transform.position - lookAtPosition);
        }
    }
}