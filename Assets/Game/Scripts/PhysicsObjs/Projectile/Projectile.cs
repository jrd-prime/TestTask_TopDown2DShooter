using System;
using Game.Scripts.Help;
using Game.Scripts.PhysicsObjs.Character;
using Game.Scripts.Systems;
using Game.Scripts.Systems.Projectile;
using UnityEngine;
using VContainer;

namespace Game.Scripts.PhysicsObjs.Projectile
{
    public interface IProjectile
    {
    }

    public class Projectile : PhysicsObject, IProjectile
    {
        private Action<Projectile> _poolCallback;
        private Rigidbody2D _rb;

        const float force = 50f;
        private Vector2 _preCollisionVelocity;
        private OutOfBoundsCheckSystem _outOfBoundsCheckSystem;
        private HitHandlerSystem _hitHandlerSystem;
        private bool IsOpponentHited;
        private Collider2D _collider;
        private Vector3 _previousPosition;

        [Inject]
        private void Construct(OutOfBoundsCheckSystem outOfBoundsCheckSystem, HitHandlerSystem hitHandlerSystem)
        {
            _outOfBoundsCheckSystem = outOfBoundsCheckSystem;
            _hitHandlerSystem = hitHandlerSystem;
        }

        private void OnEnable()
        {
            _rb = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
            IsOpponentHited = false;
            _collider.enabled = true;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            var go = other.gameObject;
            if (go.layer == LayerMask.NameToLayer(LayerMaskConstants.ObstaclesLayerName))
            {
                var contact = other.contacts[0];
                var normal = contact.normal;
                var currentVelocity = _preCollisionVelocity.normalized;
                var crossProduct = currentVelocity.x * normal.y - currentVelocity.y * normal.x;

                ChangeDirection(crossProduct);
            }
            else if (go.CompareTag("Damageable") && !IsOpponentHited)
            {
                Debug.LogWarning("damageable hit");
                IsOpponentHited = true;
                var damageable = go.GetComponent<IDamageable>() ?? throw new Exception("Damageable component is null");
                damageable.TakeDamage();

                var character = go.GetComponent<CharacterBase>() ??
                                throw new Exception("CharacterBase component is null");
                _hitHandlerSystem.HitTarget(character);
            }
        }

        private void FixedUpdate()
        {
            var currentPosition = _rb.position;
            float distanceMoved = Vector3.Distance(currentPosition, _previousPosition);
            _previousPosition = currentPosition;

            _preCollisionVelocity = _rb.linearVelocity;

            CheckOutOfBounds();

            if (!(distanceMoved < 0.1f)) return;
            if (!_collider.enabled) return;
            _collider.enabled = false;
        }

        private void CheckOutOfBounds()
        {
            if (_outOfBoundsCheckSystem.CheckOutOfBounds(_rb.position)) _poolCallback?.Invoke(this);
        }

        public void Launch(Vector3 muzzlePointPosition, Vector2 direction, Action<Projectile> poolCallback)
        {
            _poolCallback = poolCallback;

            transform.position = muzzlePointPosition;

            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);

            gameObject.SetActive(true);

            _rb.linearVelocity = direction.normalized * force;
        }

        private void ChangeDirection(float p)
        {
            var newDirection = p switch
            {
                < 0 => new Vector2(_preCollisionVelocity.y, -_preCollisionVelocity.x),
                > 0 => new Vector2(-_preCollisionVelocity.y, _preCollisionVelocity.x),
                _ => default
            };

            _rb.linearVelocity = newDirection.normalized * _preCollisionVelocity.magnitude;
        }
    }

    public interface IDamageable
    {
        // Не нужно кол-во урона
        public void TakeDamage() => Debug.LogWarning($"I take damage and die! {GetType()}");
    }
}
