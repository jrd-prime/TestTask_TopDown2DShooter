using Game.Scripts.Help;
using Game.Scripts.PhysicsObjs.Projectile;
using Game.Scripts.Settings.Main;
using UnityEngine;

namespace Game.Scripts.Settings
{
    [CreateAssetMenu(fileName = "ProjectileSettings", menuName = AssetMenuConstants.ProjectileSettings, order = 0)]
    public class ProjectileSettings : InGameSettings
    {
        public Projectile prefab;
        [Range(1, 100)] public float force = 50f;
    }
}
