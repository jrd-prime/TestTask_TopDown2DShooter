using Game.Scripts.PhysicsObjs.Player;
using Game.Scripts.Weapon;
using UnityEngine;

namespace Game.Scripts.PhysicsObjs.Enemy
{
    public interface IEnemy
    {
    }

    public class Enemy : PhysicsObject, IEnemy
    {
        public void SetWeapon(EnemyWeapon enemyWeapon)
        {
        }
    }
}
