using Game.Scripts.Weapon;
using UnityEngine;

namespace Game.Scripts.PhysicsObjs.Player
{
    public interface IPlayer
    {
        public void Fire();
        public void Move(Vector2 moveSpeed);
        public void Rotate(float angle);
        public Vector2 GetPosition();
        public void SetWeapon(IWeapon playerWeapon);
    }

    public class Player : PhysicsObject, IPlayer
    {
        [SerializeField] private Transform muzzlePoint;

        private IWeapon _weapon;
        private Rigidbody2D _rb;

        private void Start()
        {
            Debug.LogWarning("Player Start");
            _rb = GetComponent<Rigidbody2D>();
        }

        public void Fire()
        {
            _weapon.Fire(transform.right, muzzlePoint);
        }

        public void Move(Vector2 moveSpeed) => _rb.AddForce(moveSpeed);
        public void Rotate(float angle) => _rb.MoveRotation(angle);
        public Vector2 GetPosition() => _rb.position;
        public void SetWeapon(IWeapon playerWeapon) => _weapon = playerWeapon;
    }
}
