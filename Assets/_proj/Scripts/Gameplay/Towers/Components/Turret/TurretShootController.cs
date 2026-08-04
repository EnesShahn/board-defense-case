using System;
using Game.Enemies.Components;
using Game.Towers.ConfigDatas;
using Game.Towers.Root;
using LitMotion;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Towers.Components
{
    public class TurretShootController : MonoBehaviour, ITowerComponent
    {
        [SerializeField] private TowerRoot _towerRoot;
        [SerializeField] private TowerEnemyDetector _towerEnemyDetector;
        [SerializeField] private Transform _bulletSpawnPoint;
        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private Image _reloadImage;

        private TowerRoot _targetTowerRoot;
        private TowerFireRateConfigData _towerFireRateConfigData;
        private TowerDamageConfigData _towerDamageConfigData;

        private float _fireTimer;

        public TowerRoot TowerRoot => _towerRoot;

        public event Action KilledEnemy;

        private void Awake()
        {
            _towerFireRateConfigData = _towerRoot.TowerConfig.GetTowerData<TowerFireRateConfigData>();
            _towerDamageConfigData = _towerRoot.TowerConfig.GetTowerData<TowerDamageConfigData>();
            _fireTimer = 1;
            _reloadImage.fillAmount = 1;
        }
        private void Update()
        {
            _fireTimer += Time.deltaTime * _towerFireRateConfigData.FireRate;
            _reloadImage.fillAmount = _fireTimer;
            if (_fireTimer >= 1)
            {
                if (!_towerEnemyDetector.AnyEnemyInRange)
                    return;

                _fireTimer = 0;
                _reloadImage.fillAmount = 0;

                var targetEnemy = _towerEnemyDetector.EnemiesInRange[0];

                var bulletStartPos = _bulletSpawnPoint.position;
                var newBullet = GameObject.Instantiate(_bulletPrefab, _bulletSpawnPoint.position, Quaternion.identity);
                var motionBuilder = LMotion.Create(0, 1f, 0.2f).WithEase(Ease.Linear);
                motionBuilder.WithOnComplete(() =>
                {
                    GameObject.Destroy(newBullet);

                    if (targetEnemy != null)
                    {
                        var enemyHealthController = targetEnemy.GetComp<EnemyHealthController>();
                        enemyHealthController.ReceiveDamage(_towerDamageConfigData.Damage);
                        if (enemyHealthController.CurrentHealth == 0)
                            KilledEnemy?.Invoke();
                    }
                });
                motionBuilder.Bind((t) =>
                {
                    if (targetEnemy == null)
                    {
                        newBullet.gameObject.SetActive(false);
                        return;
                    }

                    newBullet.transform.position = Vector3.Lerp(bulletStartPos, targetEnemy.transform.position, t);
                });
            }
        }
    }
}