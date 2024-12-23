using UnityEngine;

namespace Game.Scripts.PhysicsObjs.Player
{
    [RequireComponent(typeof(SpriteRenderer), typeof(Rigidbody2D), typeof(Collider2D))]
    public abstract class PhysicsObject : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<Rigidbody2D>().gravityScale = 0;
        }
    }
}
