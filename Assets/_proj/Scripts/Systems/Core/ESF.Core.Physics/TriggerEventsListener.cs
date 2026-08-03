using ESF.Core.PriorityEvents;
using UnityEngine;

namespace ESF.Core.Physics
{
    public class TriggerEventsListener : MonoBehaviour
    {
        public readonly PriorityEvent<Collider> OnTriggerEntered = new();
        public readonly PriorityEvent<Collider> OnTriggerStaying = new();
        public readonly PriorityEvent<Collider> OnTriggerExited = new();

        private void OnTriggerEnter(Collider other)
        {
            OnTriggerEntered?.Invoke(this, other);
        }

        private void OnTriggerStay(Collider other)
        {
            OnTriggerStaying?.Invoke(this, other);
        }

        private void OnTriggerExit(Collider other)
        {
            OnTriggerExited?.Invoke(this, other);
        }
    }
}