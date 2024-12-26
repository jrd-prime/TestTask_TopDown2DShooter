using System;
using Game.Scripts.Help;
using Game.Scripts.Managers;
using Game.Scripts.PhysicsObjs.Character;
using Game.Scripts.Systems.Projectile;
using UnityEngine;
using VContainer;

namespace Game.Scripts.PhysicsObjs.Projectile
{
    public class Projectile : PhysicsObject
    {
        private ISettingsManager _settingsManager;
        private OutOfBoundsCheckSystem _outOfBoundsCheckSystem;
        private HitHandlerSystem _hitHandlerSystem;

        private Action<Projectile> _poolCallback;
        private Rigidbody2D _rb;
        private Vector2 _preCollisionVelocity;
        private bool _isOpponentTakeHit;
        private LayerMask _projectileOwner;

        [Inject]
        private void Construct(IObjectResolver resolver)
        {
            _settingsManager = resolver.Resolve<ISettingsManager>();
            _outOfBoundsCheckSystem = resolver.Resolve<OutOfBoundsCheckSystem>();
            _hitHandlerSystem = resolver.Resolve<HitHandlerSystem>();
        }

        private void OnEnable()
        {
            _rb = GetComponent<Rigidbody2D>();
            _isOpponentTakeHit = false;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            var go = other.gameObject;

            if (IsObstacle(go))
                OnObstacleHit(other);
            else if (IsOpponent(go))
                OnDamageableObjHit(go);
        }

        private static bool IsObstacle(GameObject go) =>
            go.layer == LayerMask.NameToLayer(LayerMaskConstants.ObstaclesLayerName);

        private bool IsOpponent(GameObject go)
            => go.CompareTag(TagConstants.DamageableTagName)
               && !_isOpponentTakeHit
               && go.layer != _projectileOwner.value;


        private void OnDamageableObjHit(GameObject go)
        {
            _isOpponentTakeHit = true;
            var damageable = go.GetComponent<IDamageable>() ?? throw new Exception("Damageable component is null");
            damageable.TakeDamage();

            var character = go.GetComponent<CharacterBase>() ?? throw new Exception("CharacterBase component is null");
            _hitHandlerSystem.HitTarget(character);
        }

        private void OnObstacleHit(Collision2D other)
        {
            var contact = other.contacts[0];
            var normal = contact.normal;
            var currentVelocity = _preCollisionVelocity.normalized;
            var crossProduct = currentVelocity.x * normal.y - currentVelocity.y * normal.x;

            _rb.linearVelocity = DirectionChanger.GetNewPerpendicularDirection(crossProduct, _preCollisionVelocity);
        }

        private void FixedUpdate()
        {
            _preCollisionVelocity = _rb.linearVelocity;

            CheckOutOfBounds();
        }

        private void CheckOutOfBounds()
        {
            if (_outOfBoundsCheckSystem.CheckOutOfBounds(_rb.position)) _poolCallback?.Invoke(this);
        }

        public void Launch(Vector3 muzzlePoint, Vector2 direction, Action<Projectile> poolCallback, LayerMask layerMask)
        {
            _poolCallback = poolCallback;
            transform.position = muzzlePoint;
            _projectileOwner = layerMask;

            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);

            gameObject.SetActive(true);

            _rb.linearVelocity = direction.normalized * 50f;
        }
    }
}
