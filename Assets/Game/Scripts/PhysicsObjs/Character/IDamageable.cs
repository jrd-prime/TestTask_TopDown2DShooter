using UnityEngine;

namespace Game.Scripts.PhysicsObjs.Projectile
{
    public interface IDamageable
    {
        // Не нужно кол-во урона
        public void TakeDamage() => Debug.LogWarning($"I take damage and die! {GetType()}");
    }
}
