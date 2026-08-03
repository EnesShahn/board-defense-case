using System.Collections.Generic;
using UnityEngine;

namespace ESF.Core.Physics
{
    public class ColliderGroup : MonoBehaviour
    {
        [SerializeField] private List<Collider> _colliders;

        public IReadOnlyList<Collider> Colliders => _colliders.AsReadOnly();

        public void SetCollidersState(bool value)
        {
            foreach (var myCollider in _colliders)
            {
                myCollider.enabled = value;
            }
        }

        public void SetIgnore(Collider otherCollider, bool ignore)
        {
            foreach (var ownCollider in _colliders)
            {
                UnityEngine.Physics.IgnoreCollision(ownCollider, otherCollider, ignore);
            }
        }
        public void SetIgnore(IReadOnlyList<Collider> otherColliders, bool ignore)
        {
            foreach (var ownCollider in _colliders)
            {
                foreach (var otherCollider in otherColliders)
                {
                    UnityEngine.Physics.IgnoreCollision(ownCollider, otherCollider, ignore);
                }
            }
        }
    }
}