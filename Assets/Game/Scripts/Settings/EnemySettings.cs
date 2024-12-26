using Game.Scripts.Help;
using Game.Scripts.PhysicsObjs.Character.Enemy;
using Game.Scripts.Settings.Main;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Scripts.Settings
{
    [CreateAssetMenu(fileName = "EnemySettings", menuName = AssetMenuConstants.EnemySettings, order = 0)]
    public class EnemySettings : InGameSettings
    {
        public Enemy prefab;
        [FormerlySerializedAs("_enemySpawnPosition")] public Transform enemySpawnPosition;
    }
}
