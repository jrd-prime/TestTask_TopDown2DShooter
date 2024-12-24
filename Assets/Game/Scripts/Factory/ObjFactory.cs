using UnityEngine;
using VContainer;

namespace Game.Scripts.Factory
{
    public sealed class ObjFactory
    {
        private IObjectResolver _resolver;

        [Inject]
        private void Construct(IObjectResolver resolver)
        {
            _resolver = resolver;
        }

        public T CreateAndInject<T>(T prefab, Transform spawnPositionTransform, Transform parent = null)
            where T : MonoBehaviour
        {
            var spawnPosition = spawnPositionTransform ? spawnPositionTransform.position : Vector3.zero;
            var spawnRotation = spawnPositionTransform ? spawnPositionTransform.rotation : Quaternion.identity;

            var obj = Object.Instantiate(prefab, spawnPosition, spawnRotation, parent: parent);
            var component = obj.GetComponent<T>() ?? throw new MissingComponentException(typeof(T).Name);
            _resolver.Inject(component);
            return component;
        }
    }

    public class ProjectileFactory
    {
    }
}
