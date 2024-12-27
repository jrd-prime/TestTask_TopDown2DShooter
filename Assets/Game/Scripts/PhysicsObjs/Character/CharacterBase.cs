using System;
using Game.Scripts.Weapon;
using UnityEngine;

namespace Game.Scripts.PhysicsObjs.Character
{
    public abstract class CharacterBase : PhysicsObject, ICharacter
    {
        [SerializeField] protected Transform muzzlePoint;
        public bool IsAlive { get; set; }
        public bool IsGameRunning { get; set; }

        private Vector2 _initialPosition;
        private IWeapon _weapon;

        private Action<ICharacter> _onDeathCallback;

        public void Initialize(Transform spawnTransform, IWeapon weapon, Action<ICharacter> gameManagerCallback)
        {
            SetWeapon(weapon);
            _onDeathCallback = gameManagerCallback;
            _initialPosition = spawnTransform.position;
            IsAlive = true;
        }

        public void Fire()
        {
            if (!IsGameRunning) return;

            if (_weapon == null) throw new Exception("Weapon is not assigned. Use SetWeapon method");
            _weapon.Fire(transform.right, muzzlePoint, gameObject.layer);
        }

        public Vector2 GetMuzzlePosition() => muzzlePoint.position;

        public void AddForce(Vector2 force)
        {
            if (!IsGameRunning) return;
            Rb.AddForce(force);
        }

        public void Rotate(float angle)
        {
            Rb.MoveRotation(angle);
        }

        public Transform GetTransform() => Rb.transform;

        public void SetWeapon(IWeapon weapon) => _weapon = weapon;

        public virtual void Deactivate() => gameObject.SetActive(false);

        public virtual void Activate()
        {
            gameObject.SetActive(true);

            Debug.LogWarning(Rb.transform.position);
            Debug.LogWarning(Rb.position);
            ResetCharacter();
        }

        public void ResetCharacter()
        {
            Rb.position = _initialPosition;
            IsAlive = true;
        }

        public void OnDeath()
        {
            _onDeathCallback.Invoke(this);
            IsAlive = false;
        }
    }
}
