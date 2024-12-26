using Game.Scripts.Help;
using Game.Scripts.PhysicsObjs.Projectile;
using Game.Scripts.Settings.Main;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Scripts.Settings
{
    [CreateAssetMenu(fileName = "ProjectileSettings", menuName = AssetMenuConstants.ProjectileSettings, order = 0)]
    public class ProjectileSettings : InGameSettings
    {
        public Projectile prefab;
    }
}
