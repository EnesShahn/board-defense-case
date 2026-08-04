using System;
using UnityEngine;
using Game.Bases;
using Game.Enemies.ConfigDatas;
using Game.Enemies.Root;
using LitMotion;
using UnityEngine.UI;

namespace Game.Enemies.Components
{
    public class BasicEnemyShootAtBaseController : MonoBehaviour, IEnemyComponent
    {
        [SerializeField] private EnemyRoot _enemyRoot;
        [SerializeField] private Transform _bulletSpawnPoint;
        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private Image _reloadImage;

        private BaseRoot _targetBaseRoot;
        private EnemyFireRateConfigData _enemyFireRateConfigData;
        private EnemyDamageConfigData _enemyDamageConfigData;

        private float _fireTimer;

        public EnemyRoot EnemyRoot => _enemyRoot;

        public event Action TargetBaseDestroyed;

        private void Awake()
        {
            _enemyFireRateConfigData = _enemyRoot.EnemyConfig.GetEnemyData<EnemyFireRateConfigData>();
            _enemyDamageConfigData = _enemyRoot.EnemyConfig.GetEnemyData<EnemyDamageConfigData>();
            _fireTimer = 1;
            _reloadImage.fillAmount = 1;
        }
        private void OnDisable()
        {
            _targetBaseRoot = null;
            _fireTimer = 1;
            _reloadImage.fillAmount = 1;
        }
        private void Update()
        {
            _fireTimer += Time.deltaTime * _enemyFireRateConfigData.FireRate;
            _reloadImage.fillAmount = _fireTimer;
            if (_fireTimer >= 1)
            {
                if (_targetBaseRoot == null)
                    return;

                _fireTimer = 0;
                _reloadImage.fillAmount = 0;

                var bulletStartPos = _bulletSpawnPoint.position;
                var newBullet = GameObject.Instantiate(_bulletPrefab, _bulletSpawnPoint.position, Quaternion.identity);
                var motionBuilder = LMotion.Create(0, 1f, 0.2f).WithEase(Ease.Linear);
                motionBuilder.WithOnComplete(() =>
                {
                    GameObject.Destroy(newBullet);

                    if (_targetBaseRoot != null)
                    {
                        var baseHealthController = _targetBaseRoot.GetComponentInChildren<BaseHealthController>();
                        baseHealthController.ReceiveDamage(_enemyDamageConfigData.Damage);
                    }
                });
                motionBuilder.Bind((t) =>
                {
                    if (_targetBaseRoot == null)
                    {
                        newBullet.gameObject.SetActive(false);
                        return;
                    }

                    newBullet.transform.position = Vector3.Lerp(bulletStartPos, _targetBaseRoot.transform.position, t);
                });
            }
        }

        public void SetTargetBase(BaseRoot baseRoot)
        {
            _targetBaseRoot = baseRoot;
        }
    }
}