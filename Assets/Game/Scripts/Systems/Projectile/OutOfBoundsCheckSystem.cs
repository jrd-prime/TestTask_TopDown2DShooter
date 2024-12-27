using System;
using Game.Scripts.Managers;
using UnityEngine;
using VContainer;

namespace Game.Scripts.Systems.Projectile
{
    public sealed class OutOfBoundsCheckSystem
    {
        private Camera _mainCamera;

        [Inject]
        private void Construct(ICameraManager cameraManager) => _mainCamera = cameraManager.GetMainCamera();

        public bool CheckOutOfBounds(Vector3 position)
        {
            if (!_mainCamera) throw new Exception("Camera is null");
            if (!_mainCamera.orthographic) throw new Exception("Camera is not orthographic");

            float verticalSize = _mainCamera.orthographicSize;
            float horizontalSize = verticalSize * _mainCamera.aspect;

            Vector3 cameraPosition = _mainCamera.transform.position;

            float left = cameraPosition.x - horizontalSize;
            float right = cameraPosition.x + horizontalSize;
            float bottom = cameraPosition.y - verticalSize;
            float top = cameraPosition.y + verticalSize;

            DrawBounds();

            return position.x < left || position.x > right || position.y < bottom || position.y > top;
        }

        private void DrawBounds()
        {
            if (!_mainCamera) return;

            float verticalSize = _mainCamera.orthographicSize;
            float horizontalSize = verticalSize * _mainCamera.aspect;

            Vector3 cameraPosition = _mainCamera.transform.position;

            Vector3 bottomLeft = new Vector3(cameraPosition.x - horizontalSize, cameraPosition.y - verticalSize, 0);
            Vector3 bottomRight = new Vector3(cameraPosition.x + horizontalSize, cameraPosition.y - verticalSize, 0);
            Vector3 topLeft = new Vector3(cameraPosition.x - horizontalSize, cameraPosition.y + verticalSize, 0);
            Vector3 topRight = new Vector3(cameraPosition.x + horizontalSize, cameraPosition.y + verticalSize, 0);

            Debug.DrawLine(bottomLeft, bottomRight, Color.green);
            Debug.DrawLine(bottomRight, topRight, Color.green);
            Debug.DrawLine(topRight, topLeft, Color.green);
            Debug.DrawLine(topLeft, bottomLeft, Color.green);
        }
    }
}
