using Game.Scripts.Managers;
using Game.Scripts.PhysicsObjs.Character;
using Game.Scripts.PhysicsObjs.Character.Enemy;
using R3;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Scripts.Systems.Enemy
{
    public sealed class EnemyFollowSystem : IInitializable, IFixedTickable, IPostStartable
    {
        public ReactiveProperty<Vector2> TargetPosition { get; } = new(Vector2.zero);
        private IEnemyCharacter _enemy;
        private ICharacter _player;
        private ISpawnManager _gameManager;

        [Inject]
        private void Construct(ISpawnManager spawnManager)
        {
            _gameManager = spawnManager;
        }

        public void Initialize()
        {
        }

        public void PostStart()
        {
            _enemy = _gameManager.enemy;
            _player = _gameManager.player;
            var targetTransform = _player.GetTransform();
            _enemy.SetTarget(targetTransform);
        }

        public void FixedTick()
        {
            Vector2 targetPosition = _player.GetTransform().position;
            if (TargetPosition.CurrentValue != targetPosition) TargetPosition.Value = targetPosition;
        }
    }
}
