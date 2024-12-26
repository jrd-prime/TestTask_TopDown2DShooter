using System;
using Game.Scripts.Weapon;
using UnityEngine;

namespace Game.Scripts.PhysicsObjs.Character
{
    public abstract class CharacterBase : PhysicsObject, ICharacter
    {
        [SerializeField] protected Transform muzzlePoint;

        private Vector2 _initialPosition;
        private IWeapon _weapon;

        protected void Start()
        {
            _initialPosition = Rb.position;
            Debug.LogWarning("initial position: " + _initialPosition);
        }

        public void Fire()
        {
            if (_weapon == null) throw new Exception("Weapon is not assigned. Use SetWeapon method");
            _weapon.Fire(transform.right, muzzlePoint, gameObject.layer);
        }

        public void AddForce(Vector2 force)
        {
            if (!gameObject.activeSelf) return;
            Rb.AddForce(force);
        }

        public void Rotate(float angle)
        {
            if (!gameObject.activeSelf) return;
            Rb.MoveRotation(angle);
        }

        public Transform GetTransform() => Rb.transform;

        public void SetWeapon(IWeapon weapon) => _weapon = weapon;

        public void Despawn() => gameObject.SetActive(false);
        public void Spawn() => gameObject.SetActive(true);
        public void ResetPosition() => Rb.position = _initialPosition;
    }
}
