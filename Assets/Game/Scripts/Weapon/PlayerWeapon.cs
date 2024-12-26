using Game.Scripts.Factory;
using Game.Scripts.PhysicsObjs.Projectile;

namespace Game.Scripts.Weapon
{
    public sealed class PlayerWeapon : WeaponBase
    {
        public PlayerWeapon(CustomPool<Projectile> projectilePool) : base(projectilePool)
        {
        }
    }
}
