using UnityEngine;

namespace Game.Scripts.Help
{
    public static class RotateAngle
    {
        public static float GetAngle(Vector2 from, Vector2 to)
        {
            var direction = to - from;
            return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        }
    }
}
