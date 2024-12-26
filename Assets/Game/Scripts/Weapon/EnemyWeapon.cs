using Game.Scripts.Factory;
using Game.Scripts.PhysicsObjs.Projectile;

namespace Game.Scripts.Weapon
{
    public sealed class EnemyWeapon : WeaponBase
    {
        public EnemyWeapon(CustomPool<Projectile> projectilePool) : base(projectilePool)
        {
        }
    }
}
