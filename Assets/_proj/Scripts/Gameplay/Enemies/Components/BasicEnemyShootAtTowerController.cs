using System;
using UnityEngine;
using Game.Enemies.ConfigDatas;
using Game.Enemies.Root;
using Game.Towers.Components;
using Game.Towers.Root;
using LitMotion;
using UnityEngine.UI;

namespace Game.Enemies.Components
{
    public class BasicEnemyShootAtTowerController : MonoBehaviour, IEnemyComponent
    {
        [SerializeField] private EnemyRoot _enemyRoot;
        [SerializeField] private Transform _bulletSpawnPoint;
        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private Image _reloadImage;

        private TowerRoot _targetTowerRoot;
        private EnemyFireRateConfigData _enemyFireRateConfigData;
        private EnemyDamageConfigData _enemyDamageConfigData;

        private float _fireTimer;

        public EnemyRoot EnemyRoot => _enemyRoot;

        public event Action TargetTowerDestroyed;

        private void Awake()
        {
            _enemyFireRateConfigData = _enemyRoot.EnemyConfig.GetEnemyData<EnemyFireRateConfigData>();
            _enemyDamageConfigData = _enemyRoot.EnemyConfig.GetEnemyData<EnemyDamageConfigData>();
            _fireTimer = 1;
            _reloadImage.fillAmount = 1;
        }
        private void OnDisable()
        {
            _targetTowerRoot = null;
            _fireTimer = 1;
            _reloadImage.fillAmount = 1;
        }
        private void Update()
        {
            _fireTimer += Time.deltaTime * _enemyFireRateConfigData.FireRate;
            _reloadImage.fillAmount = _fireTimer;
            if (_fireTimer >= 1)
            {
                if (_targetTowerRoot == null)
                    return;

                _fireTimer = 0;
                _reloadImage.fillAmount = 0;

                var bulletStartPos = _bulletSpawnPoint.position;
                var newBullet = GameObject.Instantiate(_bulletPrefab, _bulletSpawnPoint.position, Quaternion.identity);
                var motionBuilder = LMotion.Create(0, 1f, 0.2f).WithEase(Ease.Linear);
                motionBuilder.WithOnComplete(() =>
                {
                    GameObject.Destroy(newBullet);

                    if (_targetTowerRoot != null)
                    {
                        var towerHealthController = _targetTowerRoot.GetComp<TowerHealthController>();
                        towerHealthController.ReceiveDamage(_enemyDamageConfigData.Damage);
                    }
                });
                motionBuilder.Bind((t) =>
                {
                    if (_targetTowerRoot == null)
                    {
                        newBullet.gameObject.SetActive(false);
                        return;
                    }

                    newBullet.transform.position = Vector3.Lerp(bulletStartPos, _targetTowerRoot.transform.position, t);
                });
            }
        }

        public void SetTargetTower(TowerRoot towerRoot)
        {
            _targetTowerRoot = towerRoot;
        }
    }
}