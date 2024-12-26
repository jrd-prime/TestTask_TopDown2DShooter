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
        private IGameManager _gameManager;

        [Inject]
        private void Construct(IGameManager spawnManager)
        {
            _gameManager = spawnManager;
        }

        public void Initialize()
        {
        }

        public void FixedTick()
        {
            var targetTransform = _player.GetTransform();
            Vector2 targetPosition = targetTransform.position;
            if (TargetPosition.CurrentValue != targetPosition)
            {
                TargetPosition.Value = targetPosition;
            }
        }

        public void PostStart()
        {
            _enemy = _gameManager.enemy;
            _player = _gameManager.player;
            Debug.LogWarning("_enemy: " + _enemy + " / _player: " + _player);
            var targetTransform = _player.GetTransform();
            Debug.LogWarning("targetTransform: " + targetTransform);
            _enemy.SetTarget(targetTransform);
        }
    }
}
