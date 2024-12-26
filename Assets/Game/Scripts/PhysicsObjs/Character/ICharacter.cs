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
        public void Despawn();
        public void Spawn();
        public void ResetPosition();
    }
}
