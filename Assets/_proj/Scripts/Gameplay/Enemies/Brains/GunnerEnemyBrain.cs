using UnityEngine;
using MonsterLove.StateMachine;
using ESF.Utilities.Extensions;
using ESF.Core.Services;
using ESF.Core.Tags;
using Game.Bases;
using Game.Cells;
using Game.Enemies.Components;
using Game.Enemies.ConfigDatas;
using Game.Enemies.Root;
using Game.TowerCells;
using Game.Towers.Components;
using Game.Towers.ConfigDatas;
using Game.Towers.Root;

namespace Game.Enemies.Brains
{
    public class GunnerEnemyBrain : MonoBehaviour
    {
        [SerializeField] private EnemyRoot _enemyRoot;
        [SerializeField] private EnemyHealthController _enemyHealthController;
        [SerializeField] private EnemyDestructionController _enemyDestructionController;
        [SerializeField] private BasicEnemyShootAtTowerController _basicEnemyShootAtTowerController;
        [SerializeField] private BasicEnemyShootAtBaseController _basicEnemyShootAtBaseController;
        [SerializeField] private WaveCommandReceiver _waveCommandReceiver;
        [SerializeField] private float _minReachedDistance = 1f;
        [SerializeField] private float _distanceToIgnoreNewAddedTowers = 1f;

        private StateMachine<EnemyState, EnemyDriver> _fsm;

        private BaseRoot _targetBaseRoot;

        private int _laneIndex = -1;
        private CellLane _cellLane;
        private CellManager _cellManager;
        private CellRoot _targetCellRoot;
        private TowerRoot _targetTowerRoot;
        private int _currentCellIndex = -1;

        private float _moveSpeed;

        private void Awake()
        {
            _fsm = new StateMachine<EnemyState, EnemyDriver>(this);

            _cellManager = Service.Resolve<CellManager>();
            _targetBaseRoot = Service.Resolve<BaseRoot>();

            _moveSpeed = _enemyRoot.EnemyConfig.GetEnemyData<EnemyMoveSpeedConfigData>().MoveSpeed;

            _enemyRoot.EnemyPreCreate.AddListener(OnEnemyPreCreate);

            _waveCommandReceiver.BeginAssaultCommandReceived += BeginAssault;
        }
        private void Update()
        {
            _fsm.Driver.Update.Invoke();
        }

        private void OnEnemyPreCreate(object sender, EnemyRoot e)
        {
            _fsm.ChangeState(EnemyState.Idle);

            _enemyHealthController.ResetHealth();
            _enemyDestructionController.ResetDestroyState();
        }

        private void BeginAssault(int laneIndex)
        {
            _laneIndex = laneIndex;
            _cellLane = _cellManager.GetLane(_laneIndex);

            _currentCellIndex = _cellLane.GetCellCount(); // start with last cell index + 1, then decide will do --

            Decide();
        }

        private void Decide()
        {
            if (_currentCellIndex == 0) // we are at the front, start attacking base
            {
                _basicEnemyShootAtBaseController.SetTargetBase(_targetBaseRoot);
                var baseHealthController = _targetBaseRoot.GetComponentInChildren<BaseHealthController>();
                if (baseHealthController.CurrentHealth == 0)
                    _fsm.ChangeState(EnemyState.Idle);
                else
                    _fsm.ChangeState(EnemyState.AttackBase);

                return;
            }

            int nextCellIndex = _currentCellIndex - 1;
            var nextCell = _cellLane.GetCellRoot(nextCellIndex);
            if (nextCell.gameObject.TryGetTag<TowerCellTag>(out var currentTowerCellTag))
            {
                bool nextCellHasTower = currentTowerCellTag.TowerCellRoot.AttachedTowerRoot != null;
                if (nextCellHasTower)
                {
                    _targetTowerRoot = currentTowerCellTag.TowerCellRoot.AttachedTowerRoot;
                    _fsm.ChangeState(EnemyState.AttackTower);
                }
                else
                {
                    _targetCellRoot = nextCell;
                    _fsm.ChangeState(EnemyState.GoToCell);
                }

                return;
            }

            // Assume next cell just a normal cell, left here for future additions
            _targetCellRoot = nextCell;
            _fsm.ChangeState(EnemyState.GoToCell);
        }

        #region States
        private void GoToCell_Update()
        {
            var moveToPosInCell = _targetCellRoot.transform.position.WithY(_enemyRoot.transform.position.y);
            _enemyRoot.transform.position = Vector3.MoveTowards(_enemyRoot.transform.position, moveToPosInCell, _moveSpeed * Time.deltaTime);
            var lookDirection = _enemyRoot.transform.position - moveToPosInCell;
            _enemyRoot.transform.rotation = Quaternion.LookRotation(-lookDirection);

            var distanceToTargetCell = _enemyRoot.transform.position.DistanceTo(moveToPosInCell);

            if (_targetCellRoot.gameObject.TryGetTag<TowerCellTag>(out var towerCellTag))
            {
                // a Tower has been added mid walk
                if (towerCellTag.TowerCellRoot.AttachedTowerRoot != null && distanceToTargetCell > _distanceToIgnoreNewAddedTowers)
                {
                    Decide();
                    return;
                }
            }

            if (distanceToTargetCell <= _minReachedDistance)
            {
                _currentCellIndex--;
                Decide();
            }
        }
        private void AttackTower_Update()
        {
            var lookAtPosition = _targetTowerRoot.transform.position.WithY(_targetTowerRoot.transform.position.y);
            var lookDirection = _enemyRoot.transform.position - lookAtPosition;
            _enemyRoot.transform.rotation = Quaternion.LookRotation(-lookDirection);
            _basicEnemyShootAtTowerController.SetTargetTower(_targetTowerRoot);

            var targetTowerHealthController = _targetTowerRoot.GetComp<TowerHealthController>();
            if (targetTowerHealthController.CurrentHealth == 0 || !_targetTowerRoot.isActiveAndEnabled)
            {
                _targetTowerRoot = null;
                _basicEnemyShootAtTowerController.SetTargetTower(null);
                Decide();
            }
        }
        private void AttackBase_Update()
        {
            var baseHealthController = _targetBaseRoot.GetComponentInChildren<BaseHealthController>();
            if (baseHealthController.CurrentHealth == 0)
            {
                Decide();
                return;
            }

            var lookAtPosition = _targetBaseRoot.transform.position.WithY(_enemyRoot.transform.position.y).WithX(_enemyRoot.transform.position.x);
            var lookDirection = _enemyRoot.transform.position - lookAtPosition;
            _enemyRoot.transform.rotation = Quaternion.LookRotation(-lookDirection);

            _basicEnemyShootAtBaseController.SetTargetBase(_targetBaseRoot);
        }
        #endregion
    }

    public enum EnemyState
    {
        Idle,
        GoToCell,
        AttackTower,
        AttackBase,
    }

    public class EnemyDriver
    {
        public StateEvent Update;
    }
}