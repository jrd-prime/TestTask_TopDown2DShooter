using Game.Scripts.Help;
using JetBrains.Annotations;
using UnityEngine;

namespace Game.Scripts.Systems.Enemy
{
    [UsedImplicitly]
    public sealed class EnemyFiringDecisionSystem
    {
        private readonly Vector2 _projectileColliderSize = new(0.2f, 0.2f);


        public bool IsTargetInSight(Vector2 muzzlePosition, Transform targetTransform)
        {
            Vector2 targetPosition = targetTransform.position;

            var directionToTarget = (targetPosition - muzzlePosition).normalized;
            var distanceToTarget = Vector2.Distance(muzzlePosition, targetPosition);

            DrawBoxCast(muzzlePosition, directionToTarget, distanceToTarget);
            // Ignore self layer
            var layerMask = ~(1 << LayerMask.NameToLayer(LayerMaskConstants.EnemyLayerName));

            var hit = Physics2D.BoxCast(muzzlePosition, _projectileColliderSize, 0f, directionToTarget,
                distanceToTarget, layerMask);

            if (hit.collider == null) return false;
            return hit.collider.gameObject.layer == LayerMask.NameToLayer(LayerMaskConstants.PlayerLayerName);
        }


        private void DrawBoxCast(Vector2 muzzlePosition, Vector2 directionToTarget, float distanceToTarget)
        {
            Vector2 halfSize = _projectileColliderSize / 2f;

            Vector2 right = new Vector2(directionToTarget.y, -directionToTarget.x).normalized * halfSize.x;
            Vector2 up = directionToTarget * halfSize.y;
            var du = 1f;
            Debug.DrawLine(muzzlePosition + right + up,
                muzzlePosition + right - up + directionToTarget * distanceToTarget, Color.blue, du);
            Debug.DrawLine(muzzlePosition - right + up,
                muzzlePosition - right - up + directionToTarget * distanceToTarget, Color.blue, du);
            Debug.DrawLine(muzzlePosition - right - up + directionToTarget * distanceToTarget,
                muzzlePosition + right - up + directionToTarget * distanceToTarget, Color.blue, du);
            Debug.DrawLine(muzzlePosition + right + up,
                muzzlePosition + right + up + directionToTarget * distanceToTarget, Color.blue, du);
        }
    }
}
