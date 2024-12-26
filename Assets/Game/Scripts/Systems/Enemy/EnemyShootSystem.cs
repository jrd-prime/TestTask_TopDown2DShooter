﻿using System;
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
        private IGameManager _gameManager;
        private EnemyFollowSystem _enemyFollowSystem;

        private Vector2 _targetPosition;
        private bool _isFiringInProgress;
        private Action _isFiringEndCallback;
        private readonly Vector2 _projectileColliderSize = new(0.2f, 0.2f);

        private readonly CompositeDisposable _disposables = new();

        [Inject]
        private void Construct(IGameManager gameManager, EnemyFollowSystem enemyFollowSystem)
        {
            _gameManager = gameManager;
            _enemyFollowSystem = enemyFollowSystem;
        }

        public void Initialize()
        {
            _enemyFollowSystem.TargetPosition.Subscribe(SetTargetPosition).AddTo(_disposables);
            _isFiringEndCallback += OnFireEnd;
        }

        public void PostStart() => _enemy = _gameManager.enemy;

        public void FixedTick()
        {
            if (_enemy == null || _isFiringInProgress) return;
            if (IsTargetInSight())
            {
                Debug.Log("<color=green>Target In Sight!</color>");
                _enemy.StopAndFire(_isFiringEndCallback);
                _isFiringInProgress = true;
            }
            else
            {
                //TODO Target is not in sight. Mb ricochet?
            }
        }

        private void DrawBoxCast(Vector2 muzzlePosition, Vector2 directionToTarget, float distanceToTarget)
        {
            // Размеры бокса
            Vector2 halfSize = _projectileColliderSize / 2f;

            // Вычисляем углы для рисования бокса
            Vector2 right = new Vector2(directionToTarget.y, -directionToTarget.x).normalized * halfSize.x;
            Vector2 up = directionToTarget * halfSize.y;

            // Рисуем 4 линии для бокса (длина будет до цели)
            Debug.DrawLine(muzzlePosition + right + up,
                muzzlePosition + right - up + directionToTarget * distanceToTarget, Color.blue, 0.1f);
            Debug.DrawLine(muzzlePosition - right + up,
                muzzlePosition - right - up + directionToTarget * distanceToTarget, Color.blue, 0.1f);
            Debug.DrawLine(muzzlePosition - right - up + directionToTarget * distanceToTarget,
                muzzlePosition + right - up + directionToTarget * distanceToTarget, Color.blue, 0.1f);
            Debug.DrawLine(muzzlePosition + right + up,
                muzzlePosition + right + up + directionToTarget * distanceToTarget, Color.blue, 0.1f);
        }


        private bool IsTargetInSight()
        {
            var muzzlePosition = _enemy.GetMuzzlePosition();
            var directionToTarget = (_targetPosition - muzzlePosition).normalized;
            var distanceToTarget = Vector2.Distance(muzzlePosition, _targetPosition);

            DrawBoxCast(muzzlePosition, directionToTarget, distanceToTarget);
            // Ignore self layer
            var layerMask = ~(1 << LayerMask.NameToLayer(LayerMaskConstants.EnemyLayerName));

            var hit = Physics2D.BoxCast(muzzlePosition, _projectileColliderSize, 0f, directionToTarget,
                distanceToTarget, layerMask);

            if (hit.collider == null) return false;
            return hit.collider.gameObject.layer == LayerMask.NameToLayer(LayerMaskConstants.PlayerLayerName);
        }

        private void SetTargetPosition(Vector2 targetPosition) => _targetPosition = targetPosition;
        private void OnFireEnd() => _isFiringInProgress = false;
    }
}
