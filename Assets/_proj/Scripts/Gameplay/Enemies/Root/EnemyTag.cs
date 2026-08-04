using ESF.Core.Tags;
using UnityEngine;

namespace Game.Enemies.Root
{
    [DisallowMultipleComponent]
    public class EnemyTag : Tag<EnemyTag>, ITag
    {
        [SerializeField] private EnemyRoot _enemyRoot;

        public EnemyRoot EnemyRoot => _enemyRoot;

        private void OnValidate()
        {
            _enemyRoot ??= GetComponent<EnemyRoot>();
            _enemyRoot ??= GetComponentInParent<EnemyRoot>();
        }
    }
}