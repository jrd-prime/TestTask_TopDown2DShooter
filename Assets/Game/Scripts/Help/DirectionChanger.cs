using UnityEngine;

namespace Game.Scripts.Help
{
    public static class DirectionChanger
    {
        public static Vector2 GetNewPerpendicularDirection(float crossProduct, Vector2 preCollisionVelocity)
        {
            return crossProduct switch
            {
                < 0 => new Vector2(preCollisionVelocity.y, -preCollisionVelocity.x),
                > 0 => new Vector2(-preCollisionVelocity.y, preCollisionVelocity.x),
                _ => default
            };
        }
    }
}
