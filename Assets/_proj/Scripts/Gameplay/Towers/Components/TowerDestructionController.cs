using System;
using Cysharp.Threading.Tasks;
using LitMotion;
using UnityEngine;
using ESF.Core.Services;
using Game.Towers.Configs;
using Game.Towers.Root;

namespace Game.Towers.Components
{
    public class TowerDestructionController : MonoBehaviour, ITowerComponent
    {
        [SerializeField] private TowerRoot _towerRoot;
        [SerializeField] private GameObject _towerModel;
        [SerializeField] private TowerHealthController _towerHealthController;
        [SerializeField] private float _scaleDownAnimationDuration = 0.3f;

        private bool _isDestroyed;
        private bool _isDestroying;

        private TowerConfigService _towerConfigService;

        public TowerRoot TowerRoot => _towerRoot;

        public event Action<TowerDestructionController> TowerDestroyed;

        private void Awake()
        {
            _towerConfigService = Service.Resolve<TowerConfigService>();

            _towerHealthController.HealthReachedZero += OnTowerHealthReachedZero;
        }
        private void OnTowerHealthReachedZero(TowerHealthController towerHealthController)
        {
            if (_isDestroying || _isDestroyed)
                return;
            
            _isDestroying = true;

            DestroyTower();
        }

        private async UniTaskVoid DestroyTower()
        {
            await LMotion.Create(1f, 0, _scaleDownAnimationDuration).WithEase(Ease.OutQuad)
                .Bind((t) => { _towerModel.transform.localScale = new Vector3(t, t, t); });

            _towerConfigService.DestroyTower(_towerRoot);

            _towerModel.transform.localScale = Vector3.one;

            _isDestroyed = true;
            _isDestroying = false;

            TowerDestroyed?.Invoke(this);
        }

        public void ResetDestroyState()
        {
            _isDestroyed = false;
            _isDestroying = false;
        }
    }
}