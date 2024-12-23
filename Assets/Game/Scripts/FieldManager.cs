using System;
using UnityEngine;

namespace Game.Scripts
{
    public class FieldManager : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer field;
        [SerializeField] private Camera mainCamera;

        private void Awake()
        {
            if (mainCamera == null) throw new Exception("Camera is null");
        }

        private void Start()
        {
            ScaleSpriteToScreen();
        }

        private void ScaleSpriteToScreen()
        {
            var worldHeight = 2f * mainCamera.orthographicSize;
            var worldWidth = worldHeight * mainCamera.aspect;

            var pixelsToUnits =
                1f / mainCamera.pixelHeight * worldHeight;
            var paddingInWorldUnits = 16f * pixelsToUnits;

            worldHeight -= 2f * paddingInWorldUnits;
            worldWidth -= 2f * paddingInWorldUnits;

            Debug.LogWarning($"width: {worldWidth}, height: {worldHeight}");

            Vector2 spriteSize = field.sprite.bounds.size;

            var scaleX = worldWidth / spriteSize.x;
            var scaleY = worldHeight / spriteSize.y;


            field.transform.localScale = new Vector3(scaleX, scaleY, 1f);

            CreateEdgeCollider(new Vector2(0, worldHeight / 2f), new Vector2(worldWidth / 2f, worldHeight / 2f),
                new Vector2(-worldWidth / 2f, worldHeight / 2f)); // Верхний край
            CreateEdgeCollider(new Vector2(0, -worldHeight / 2f), new Vector2(worldWidth / 2f, -worldHeight / 2f),
                new Vector2(-worldWidth / 2f, -worldHeight / 2f)); // Нижний край
            CreateEdgeCollider(new Vector2(worldWidth / 2f, 0), new Vector2(worldWidth / 2f, worldHeight / 2f),
                new Vector2(worldWidth / 2f, -worldHeight / 2f)); // Правый край
            CreateEdgeCollider(new Vector2(-worldWidth / 2f, 0), new Vector2(-worldWidth / 2f, worldHeight / 2f),
                new Vector2(-worldWidth / 2f, -worldHeight / 2f)); // Левый край
        }

        private static void CreateEdgeCollider(Vector2 topLeft, Vector2 topRight, Vector2 bottomLeft)
        {
            var gameObj = new GameObject("ColliderEdge");
            var edgeCollider = gameObj.AddComponent<EdgeCollider2D>();

            edgeCollider.points = new[] { topLeft, topRight, bottomLeft };

            gameObj.transform.position = Vector3.zero;
        }
    }
}
