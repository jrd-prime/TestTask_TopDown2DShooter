using System;
using System.Threading.Tasks;
using Game.Scripts.Help;
using Game.Scripts.Managers;
using Game.Scripts.PhysicsObjs.Character.Enemy;
using R3;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Scripts.Systems.Enemy
{
    public sealed class EnemyShootSystem : IInitializable, IFixedTickable, IPostStartable
    {
        private IEnemyCharacter _enemy;
        private ISpawnManager _gameManager;
        private EnemyFollowSystem _enemyFollowSystem;
        private Vector2 _targetPosition;
        private bool _isFiringInProgress;
        private readonly CompositeDisposable _disposables = new();
        private Action _isFiringEndCallback;
        private Vector2 _projectileColliderSize = new(0.1f, 0.1f);

        [Inject]
        private void Construct(ISpawnManager spawnManager, EnemyFollowSystem enemyFollowSystem)
        {
            _gameManager = spawnManager;
            _enemyFollowSystem = enemyFollowSystem;
        }

        public void Initialize()
        {
            _enemyFollowSystem.TargetPosition.Subscribe(x => _targetPosition = x).AddTo(_disposables);
            _isFiringEndCallback += OnFireEnd;
        }

        private void OnFireEnd() => _isFiringInProgress = false;

        public void FixedTick()
        {
            if (_enemy == null || _isFiringInProgress) return;

            if (!IsTargetInSight()) return;
            Debug.Log("<color=green>Target In Sight!</color>");
            _enemy.StopAndFire(_isFiringEndCallback);
            _isFiringInProgress = true;
        }

        private bool IsTargetInSight()
        {
            var muzzlePosition = _enemy.GetMuzzlePosition();
            var directionToTarget = (_targetPosition - muzzlePosition).normalized;
            var distanceToTarget = Vector2.Distance(muzzlePosition, _targetPosition);

            // Ignore self layer
            var layerMask = ~(1 << LayerMask.NameToLayer(LayerMaskConstants.EnemyLayerName));

            var hit = Physics2D.BoxCast(muzzlePosition, _projectileColliderSize, 0f, directionToTarget,
                distanceToTarget, layerMask);

            if (hit.collider == null) return false;
            return hit.collider.gameObject.layer == LayerMask.NameToLayer(LayerMaskConstants.PlayerLayerName);
        }

        public void PostStart() => _enemy = _gameManager.enemy;
    }
}
