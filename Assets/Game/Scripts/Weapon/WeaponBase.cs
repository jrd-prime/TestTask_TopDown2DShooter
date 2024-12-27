using System;
using Game.Scripts.Factory;
using Game.Scripts.PhysicsObjs.Projectile;
using UnityEngine;

namespace Game.Scripts.Weapon
{
    public interface IWeapon
    {
        public void Fire(Vector2 direction, Transform muzzlePoint, LayerMask layerMask);
    }

    public abstract class WeaponBase : IWeapon
    {
        private readonly CustomPool<Projectile> _projectilePool;
        private readonly Action<Projectile> _poolCallback;

        protected WeaponBase(CustomPool<Projectile> projectilePool)
        {
            _projectilePool = projectilePool;
            _poolCallback = PoolCallback;
        }

        public void Fire(Vector2 direction, Transform muzzlePoint, LayerMask layerMask)
        {
            var projectile = _projectilePool.Get();
            projectile.Launch(muzzlePoint.position, direction, _poolCallback, layerMask);
        }

        private void PoolCallback(Projectile projectile) => _projectilePool.Return(projectile);
    }
}
