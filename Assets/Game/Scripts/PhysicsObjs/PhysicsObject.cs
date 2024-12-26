using UnityEngine;

namespace Game.Scripts.PhysicsObjs
{
    [RequireComponent(typeof(SpriteRenderer), typeof(Rigidbody2D), typeof(Collider2D))]
    public abstract class PhysicsObject : MonoBehaviour
    {
        protected Rigidbody2D Rb;

        protected void Awake()
        {
            Rb = GetComponent<Rigidbody2D>();
            Rb.gravityScale = 0;
        }
    }
}
