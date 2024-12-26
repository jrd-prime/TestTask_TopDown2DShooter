using System;
using UnityEngine;

namespace Game.Scripts.Managers
{
    public class FieldManager : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer field;
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private Camera _mainCamera;
        
        private void Start()
        {
            if (_mainCamera == null) throw new Exception("Camera is null");
            ScaleSpriteToScreen();
        }

        private void ScaleSpriteToScreen()
        {
            var worldHeight = 2f * _mainCamera.orthographicSize;
            var worldWidth = worldHeight * _mainCamera.aspect;

            var pixelsToUnits = 1f / _mainCamera.pixelHeight * worldHeight;
            var paddingInWorldUnits = 16f * pixelsToUnits;

            // worldHeight -= 2f * paddingInWorldUnits;
            // worldWidth -= 2f * paddingInWorldUnits;

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

            gameObj.layer = LayerMask.NameToLayer("Walls");
        }
    }
}
