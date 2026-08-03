using System;
using Cysharp.Threading.Tasks;
using LitMotion;
using UnityEngine;
using ESF.Core.Services;
using Game.Enemies.Configs;
using Game.Enemies.Root;

namespace Game.Enemies.Components
{
    public class EnemyDestructionController : MonoBehaviour, IEnemyComponent
    {
        [SerializeField] private EnemyRoot _enemyRoot;
        [SerializeField] private GameObject _enemyModel;
        [SerializeField] private EnemyHealthController _enemyHealthController;
        [SerializeField] private float _scaleDownAnimationDuration = 0.3f;

        private bool _isDestroyed;
        private bool _isDestroying;

        private EnemyConfigService _enemyConfigService;

        public EnemyRoot EnemyRoot => _enemyRoot;

        public event Action<EnemyDestructionController> EnemyDestroyed;

        private void Awake()
        {
            _enemyConfigService = Service.Resolve<EnemyConfigService>();

            _enemyHealthController.HealthReachedZero += OnEnemyHealthReachedZero;
        }
        private void OnEnemyHealthReachedZero(EnemyHealthController enemyHealthController)
        {
            if (_isDestroying || _isDestroyed)
                return;
            _isDestroying = true;

            DestroyEnemy();
        }

        private async UniTaskVoid DestroyEnemy()
        {
            await LMotion.Create(1f, 0, _scaleDownAnimationDuration).WithEase(Ease.OutQuad)
                .Bind((t) => { _enemyModel.transform.localScale = new Vector3(t, t, t); });

            _enemyConfigService.DestroyEnemy(_enemyRoot);

            _enemyModel.transform.localScale = Vector3.one;

            _isDestroyed = true;
            _isDestroying = false;

            EnemyDestroyed?.Invoke(this);
        }

        public void ResetDestroyState()
        {
            _isDestroyed = false;
            _isDestroying = false;
        }
    }
}