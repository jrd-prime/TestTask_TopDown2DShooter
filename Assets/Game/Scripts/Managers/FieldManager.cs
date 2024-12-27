using System;
using Game.Scripts.Help;
using UnityEngine;

namespace Game.Scripts.Managers
{
    public class FieldManager : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer field;
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private Camera mainCamera;

        private void Start()
        {
            if (mainCamera == null) throw new Exception("Camera is null");
            ScaleFieldToScreen();
        }

        private void ScaleFieldToScreen()
        {
            var worldHeight = 2f * mainCamera.orthographicSize;
            var worldWidth = worldHeight * mainCamera.aspect;

            Vector2 spriteSize = field.sprite.bounds.size;

            var scaleX = worldWidth / spriteSize.x;
            var scaleY = worldHeight / spriteSize.y;

            field.transform.localScale = new Vector3(scaleX, scaleY, 1f);

            CreateWorldEdges(transform, worldWidth, worldHeight);
        }

        private static void CreateWorldEdges(Transform transform, float worldWidth, float worldHeight)
        {
            var topLeft = new Vector2(-worldWidth / 2f, worldHeight / 2f);
            var topRight = new Vector2(worldWidth / 2f, worldHeight / 2f);
            var bottomLeft = new Vector2(-worldWidth / 2f, -worldHeight / 2f);
            var bottomRight = new Vector2(worldWidth / 2f, -worldHeight / 2f);
            var middleTop = new Vector2(0, worldHeight / 2f);
            var middleBottom = new Vector2(0, -worldHeight / 2f);
            var middleRight = new Vector2(worldWidth / 2f, 0);
            var middleLeft = new Vector2(-worldWidth / 2f, 0);

            CreateEdgeCollider(transform, topLeft, topRight, middleTop, "Top");
            CreateEdgeCollider(transform, bottomLeft, bottomRight, middleBottom, "Bottom");
            CreateEdgeCollider(transform, topRight, bottomRight, middleRight, "Right");
            CreateEdgeCollider(transform, topLeft, bottomLeft, middleLeft, "Left");
        }

        private static void CreateEdgeCollider(Transform parent, Vector2 tLeft, Vector2 tRight, Vector2 bLeft,
            string edgeName)
        {
            var gameObj = new GameObject
            {
                name = edgeName,
                layer = LayerMask.NameToLayer(LayerMaskConstants.WallsLayerName),
                transform =
                {
                    position = Vector3.zero,
                    parent = parent
                }
            };

            gameObj.AddComponent<EdgeCollider2D>().points = new[] { tLeft, tRight, bLeft };
        }
    }
}
