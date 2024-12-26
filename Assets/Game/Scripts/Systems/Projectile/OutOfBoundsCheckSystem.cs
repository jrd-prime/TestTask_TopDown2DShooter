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
            if (_mainCamera == null) throw new Exception("Camera is null");
            var viewportPos = _mainCamera.WorldToViewportPoint(position);
            return viewportPos.x < 0 || viewportPos.x > 1 || viewportPos.y < 0 || viewportPos.y > 1;
        }
    }
}
