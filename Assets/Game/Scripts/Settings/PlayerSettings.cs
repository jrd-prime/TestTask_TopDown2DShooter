using System;
using Game.Scripts.Help;
using Game.Scripts.PhysicsObjs.Character.Player;
using Game.Scripts.Settings.Main;
using UnityEngine;

namespace Game.Scripts.Settings
{
    [CreateAssetMenu(fileName = "PlayerSettings", menuName = AssetMenuConstants.PlayerSettings, order = 0)]
    public class PlayerSettings : InGameSettings
    {
        public Player prefab;
        public Transform playerSpawnPosition;
        [Range(2f, 10f)] public float playerSpeed = 5f;

        private void OnValidate()
        {
            if (prefab == null) throw new Exception("Prefab is null");
        }
    }
}
