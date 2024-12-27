using System;
using Game.Scripts.PhysicsObjs.Projectile;
using Game.Scripts.Weapon;
using UnityEngine;

namespace Game.Scripts.PhysicsObjs.Character
{
    public interface ICharacter : IDamageable
    {
        public void Fire();
        public void AddForce(Vector2 force);
        public void Rotate(float angle);
        public Transform GetTransform();
        public void SetWeapon(IWeapon weapon);
        public void Deactivate();
        public void Activate();
        public void ResetCharacter();
        public void OnDeath();
        public void Initialize(Transform spawnTransform, IWeapon weapon, Action<ICharacter> gameManagerCallback);
        public bool IsAlive { get; set; }
        public bool IsGameRunning { get; set; }
        public Vector2 GetMuzzlePosition();
    }
}
