using System;
using System.Threading.Tasks;
using Pathfinding;
using UnityEngine;

namespace Game.Scripts.PhysicsObjs.Character.Enemy
{
    [RequireComponent(typeof(AIPath), typeof(AIDestinationSetter))]
    public class Enemy : CharacterBase, IEnemyCharacter
    {
        private AIDestinationSetter a;
        private AIPath b;

        protected new void Awake()
        {
            base.Awake();

            a = GetComponent<AIDestinationSetter>();
            b = GetComponent<AIPath>();
        }

        public void SetTarget(Transform targetPosition)
        {
            if (a == null) throw new Exception("AIDestinationSetter is null");

            a.target = targetPosition;
            Debug.LogWarning("Set target position ");
        }

        public async void StopAndFire(Action isFiringEndCallback)
        {
            b.canMove = false;

            await Aim();

            Fire();
            b.canMove = true;
            isFiringEndCallback?.Invoke();
        }

        public Vector2 GetMuzzlePosition() => muzzlePoint.position;

        private static async Task Aim() => await Task.Delay(500);
    }

    public interface IEnemyCharacter : ICharacter
    {
        public void SetTarget(Transform targetPosition);
        public void StopAndFire(Action isFiringEndCallback);
        public Vector2 GetMuzzlePosition();
    }
}
