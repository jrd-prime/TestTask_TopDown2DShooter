using System;
using System.Collections.Generic;
using Game.Scripts.Help;
using JetBrains.Annotations;
using UnityEngine;

namespace Game.Scripts.Settings
{
    [CreateAssetMenu(fileName = "MainSettingsList", menuName = AssetMenuConstants.MainSettings, order = 0)]
    public class MainSettings : ScriptableObject
    {
        public PlayerSettings playerSettings;
        public EnemySettings enemySettings;
        public ProjectileSettings projectileSettings;

        public Dictionary<Type, InGameSettings> settingsDictionary { get; } = new();

        private void OnValidate()
        {
            if (playerSettings == null || enemySettings == null || projectileSettings == null)
            {
                throw new Exception("Settings are not assigned");
            }
        }

        public Dictionary<Type, InGameSettings> GetSettingsList()
        {
            settingsDictionary.Add(typeof(PlayerSettings), playerSettings);
            settingsDictionary.Add(typeof(EnemySettings), enemySettings);
            settingsDictionary.Add(typeof(ProjectileSettings), projectileSettings);

            return settingsDictionary;
        }
    }
}
