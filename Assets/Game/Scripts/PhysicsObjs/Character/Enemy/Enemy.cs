using System;
using System.Threading.Tasks;
using Game.Scripts.Systems.Enemy;
using Pathfinding;
using UnityEngine;
using VContainer;

namespace Game.Scripts.PhysicsObjs.Character.Enemy
{
    public interface IEnemyCharacter : ICharacter
    {
        public void SetTarget(Transform targetTransform);
    }

    [RequireComponent(typeof(AIPath), typeof(AIDestinationSetter))]
    public class Enemy : CharacterBase, IEnemyCharacter
    {
        private AIDestinationSetter _destinationSetter;
        private AIPath _aiPath;
        private EnemyFiringDecisionSystem _enemyFiringDecisionSystem;
        private Transform _targetTransform;
        private bool _isFiringInProgress;

        [Inject]
        private void Construct(EnemyFiringDecisionSystem enemyFiringDecisionSystem)
        {
            _enemyFiringDecisionSystem = enemyFiringDecisionSystem;
        }

        protected new void Awake()
        {
            base.Awake();

            _destinationSetter = GetComponent<AIDestinationSetter>();
            _aiPath = GetComponent<AIPath>();
        }

        private void FixedUpdate()
        {
            _aiPath.canMove = IsGameRunning;


            if (!gameObject.activeSelf || _isFiringInProgress) return;

            var muzzlePosition = GetMuzzlePosition();

            if (_enemyFiringDecisionSystem.IsTargetInSight(muzzlePosition, _targetTransform))
            {
                Debug.Log("<color=green>Target In Sight!</color>");
                StopAndFire();
            }
            else
            {
                //TODO Target is not in sight. Mb ricochet?
            }
        }

        public void SetTarget(Transform targetTransform)
        {
            _targetTransform = targetTransform;

            if (_destinationSetter == null) throw new Exception("AIDestinationSetter is null");

            _destinationSetter.target = _targetTransform;
        }

        public async void StopAndFire()
        {
            if (_isFiringInProgress) return;

            _isFiringInProgress = true;
            _aiPath.canMove = false;

            await Aim();

            Fire();

            _aiPath.canMove = true;
            _isFiringInProgress = false;
        }

        private static async Task Aim() => await Task.Delay(500);
    }
}
