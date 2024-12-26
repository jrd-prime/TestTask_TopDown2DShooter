using System.Collections.Generic;
using Game.Scripts.Factory;
using Game.Scripts.PhysicsObjs.Character;
using Game.Scripts.PhysicsObjs.Character.Enemy;
using Game.Scripts.PhysicsObjs.Projectile;
using Game.Scripts.Settings;
using Game.Scripts.Weapon;
using UnityEngine;
using VContainer;

namespace Game.Scripts.Managers
{
    public interface ISpawnManager
    {
        public void SpawnUnits();
        public void DespawnUnits();
        public ICharacter player { get; }
        public IEnemyCharacter enemy { get; }
        public void ResetUnits();
        public void DespawnAll();
    }

    public class SpawnManager : MonoBehaviour, ISpawnManager
    {
        public ICharacter player { get; private set; }
        public IEnemyCharacter enemy { get; private set; }


        private ISettingsManager _settingsManager;

        private PlayerSettings _playerSettings;
        private EnemySettings _enemySettings;
        private ProjectileSettings _projectileSettings;

        private Projectile _projectile;
        private CustomPool<Projectile> _projectilePool;
        private IObjectResolver _resolver;
        public CustomPool<Projectile> projectilesPool { get; private set; }

        private readonly List<ICharacter> _units = new();

        [Inject]
        private void Construct(IObjectResolver resolver)
        {
            _resolver = resolver;
            _settingsManager = resolver.Resolve<ISettingsManager>();
            _playerSettings = _settingsManager.GetSettings<PlayerSettings>();
            _enemySettings = _settingsManager.GetSettings<EnemySettings>();
            _projectileSettings = _settingsManager.GetSettings<ProjectileSettings>();
            var factory = resolver.Resolve<ObjFactory>();

            var holder = new GameObject("ProjectilesHolder");

            projectilesPool =
                new CustomPool<Projectile>(_projectileSettings.prefab, 30, holder.transform, _resolver, true);

            player = factory.CreateAndInject(_playerSettings.prefab, _playerSettings.playerSpawnPosition);
            var playerWeapon = new PlayerWeapon(projectilesPool);
            player.SetWeapon(playerWeapon);

            enemy = factory.CreateAndInject(_enemySettings.prefab, _enemySettings.enemySpawnPosition);
            var enemyWeapon = new EnemyWeapon(projectilesPool);
            enemy.SetWeapon(enemyWeapon);

            _units.Add(player);
            _units.Add(enemy);

            DespawnUnits();
        }


        public void SpawnUnits()
        {
            foreach (var unit in _units) unit.Spawn();
        }

        public void DespawnUnits()
        {
            foreach (var unit in _units) unit.Despawn();
        }

        public void ResetUnits()
        {
            foreach (var unit in _units) unit.ResetPosition();
        }

        public void DespawnAll()
        {
            projectilesPool.ReturnAll();

            foreach (var unit in _units) unit.Despawn();
        }
    }
}
