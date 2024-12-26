using System;
using Game.Scripts.Help;
using Game.Scripts.Managers;
using Game.Scripts.PhysicsObjs.Character;
using Game.Scripts.PhysicsObjs.Character.Enemy;
using R3;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Scripts.Systems.Enemy
{
    public sealed class EnemyMovementSystem : IInitializable, IDisposable
    {
        private EnemyFollowSystem _enemyFollowSystem;
        private readonly CompositeDisposable _disposables = new();
        private ISpawnManager _gameManager;
        private IEnemyCharacter _enemy;

        [Inject]
        private void Construct(ISpawnManager spawnManager, EnemyFollowSystem enemyFollowSystem)
        {
            _gameManager = spawnManager;
            _enemy = spawnManager.enemy;
            _enemyFollowSystem = enemyFollowSystem;
        }

        public void Initialize()
        {
            _enemyFollowSystem.TargetPosition.Subscribe(MoveToTargetPosition).AddTo(_disposables);
        }

        private void MoveToTargetPosition(Vector2 targetPosition)
        {
            Rotate(targetPosition);
        }

        private void Rotate(Vector2 targetPosition)
        {
            var angle = RotateAngle.GetAngle(_enemy.GetTransform().position, targetPosition);
            _enemy.Rotate(angle);
        }

        public void Dispose() => _disposables.Dispose();
    }
}
