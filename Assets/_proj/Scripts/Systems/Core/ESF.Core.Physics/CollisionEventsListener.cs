using ESF.Core.PriorityEvents;
using UnityEngine;

namespace ESF.Core.Physics
{
    public class CollisionEventsListener : MonoBehaviour
    {
        public readonly PriorityEvent<Collision> OnCollisionEntered = new();
        public readonly PriorityEvent<Collision> OnCollisionExited = new();

        private void OnCollisionEnter(Collision other)
        {
            OnCollisionEntered?.Invoke(this, other);
        }

        private void OnCollisionExit(Collision other)
        {
            OnCollisionExited?.Invoke(this, other);
        }
    }
}