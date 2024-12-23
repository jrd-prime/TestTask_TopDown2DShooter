using UnityEngine;

namespace Game.Scripts.Factory
{
    public class ObjFactory
    {
        public static T Create<T>(T prefab, Transform spawnPositionTransform, Transform parent = null)
            where T : MonoBehaviour
        {
            var spawnPosition = spawnPositionTransform ? spawnPositionTransform.position : Vector3.zero;
            var spawnRotation = spawnPositionTransform ? spawnPositionTransform.rotation : Quaternion.identity;

            var obj = Object.Instantiate(prefab, spawnPosition, spawnRotation, parent: parent);
            return obj.GetComponent<T>() ?? throw new MissingComponentException(typeof(T).Name);
        }
    }

    public class ProjectileFactory
    {
    }
}
