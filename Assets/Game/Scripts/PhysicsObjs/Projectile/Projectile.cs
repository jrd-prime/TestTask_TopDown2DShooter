using System;
using UnityEngine;

namespace Game.Scripts.PhysicsObjs.Projectile
{
    public interface IProjectile
    {
    }

    public class Projectile : PhysicsObject, IProjectile
    {
        [SerializeField] private Camera mainCamera;
        private Action<Projectile> _poolCallback;
        private Rigidbody2D rb;

        const float force = 50f;
        private Vector2 _preCollisionVelocity;

        private void OnEnable()
        {
            if (mainCamera == null) throw new Exception("Camera is null");
            rb = GetComponent<Rigidbody2D>();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Obstacles"))
            {
                var contact = other.contacts[0];
                var normal = contact.normal;
                var currentVelocity = _preCollisionVelocity.normalized;
                var crossProduct = currentVelocity.x * normal.y - currentVelocity.y * normal.x;

                ChangeDirection(crossProduct);
            }
        }

        private void FixedUpdate()
        {
            _preCollisionVelocity = rb.linearVelocity;
            CheckOutOfBounds();
        }

        private void CheckOutOfBounds()
        {
            var viewportPos = mainCamera.WorldToViewportPoint(transform.position);

            if (viewportPos.x < 0 || viewportPos.x > 1 || viewportPos.y < 0 || viewportPos.y > 1)
                _poolCallback?.Invoke(this);
        }

        public void Launch(Vector3 muzzlePointPosition, Vector2 direction, Action<Projectile> poolCallback)
        {
            _poolCallback = poolCallback;

            transform.position = muzzlePointPosition;

            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);

            gameObject.SetActive(true);

            rb.linearVelocity = direction.normalized * force;
        }

        private void ChangeDirection(float p)
        {
            var newDirection = p switch
            {
                < 0 => new Vector2(_preCollisionVelocity.y, -_preCollisionVelocity.x),
                > 0 => new Vector2(-_preCollisionVelocity.y, _preCollisionVelocity.x),
                _ => default
            };

            rb.linearVelocity = newDirection.normalized * _preCollisionVelocity.magnitude;
        }
    }
}
