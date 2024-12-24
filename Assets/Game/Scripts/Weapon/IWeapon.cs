﻿using System;
using Game.Scripts.Factory;
using Game.Scripts.PhysicsObjs.Projectile;
using UnityEngine;

namespace Game.Scripts.Weapon
{
    public interface IWeapon
    {
        public void Fire(Vector2 direction, Transform muzzlePoint);
    }

    public abstract class WeaponBase : IWeapon
    {
        protected readonly CustomPool<Projectile> ProjectilePool;
        private readonly Action<Projectile> _poolCallback;

        protected WeaponBase(CustomPool<Projectile> projectilePool)
        {
            ProjectilePool = projectilePool;
            _poolCallback = PoolCallback;
        }

        public void Fire(Vector2 direction, Transform muzzlePoint)
        {
            Debug.LogWarning("Enemy fire with direction: " + direction);

            var projectile = ProjectilePool.Get();
            projectile.Launch(muzzlePoint.position, direction, _poolCallback);
        }

        private void PoolCallback(Projectile projectile) => ProjectilePool.Return(projectile);
    }

    public sealed class PlayerWeapon : WeaponBase
    {
        public PlayerWeapon(CustomPool<Projectile> projectilePool) : base(projectilePool)
        {
        }
    }

    public sealed class EnemyWeapon : WeaponBase
    {
        public EnemyWeapon(CustomPool<Projectile> projectilePool) : base(projectilePool)
        {
        }
    }
}
