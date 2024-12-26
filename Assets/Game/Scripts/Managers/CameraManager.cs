using System;
using UnityEngine;

namespace Game.Scripts.Managers
{
    public sealed class CameraManager : MonoBehaviour, ICameraManager
    {
        [SerializeField] private Camera mainCamera;

        private void Awake()
        {
            if (mainCamera == null) throw new NullReferenceException("Camera is not set!");
        }

        public Camera GetMainCamera() => mainCamera;
    }

    public interface ICameraManager
    {
        public Camera GetMainCamera();
    }
}
