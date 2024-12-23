using System;
using Game.Scripts.Factory;
using Game.Scripts.PhysicsObjs.Enemy;
using Game.Scripts.PhysicsObjs.Player;
using Game.Scripts.PhysicsObjs.Projectile;
using Game.Scripts.Settings;
using JetBrains.Annotations;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Scripts.Managers
{
    public interface ISpawnManager
    {
        public void SpawnUnits();
        public void DespawnUnits();
    }


    public interface IProjectileSpawnManager
    {
        public void SpawnProjectile();
    }

    public class SpawnManager : MonoBehaviour, ISpawnManager, IProjectileSpawnManager
    {
        private ISettingsManager _settingsManager;

        private PlayerSettings _playerSettings;
        private EnemySettings _enemySettings;
        private ProjectileSettings _projectileSettings;

        private Player _player;
        private Enemy _enemy;
        private Projectile _projectile;
        private CustomPool<Projectile> _projectilePool;
        private IObjectResolver _resolver;

        [Inject]
        private void Construct(IObjectResolver resolver)
        {
            _resolver = resolver;
            _settingsManager = resolver.Resolve<ISettingsManager>();
            _playerSettings = _settingsManager.GetSettings<PlayerSettings>();
            _enemySettings = _settingsManager.GetSettings<EnemySettings>();
            _projectileSettings = _settingsManager.GetSettings<ProjectileSettings>();


            _player = resolver.Resolve<Player>();
            _enemy = resolver.Resolve<Enemy>();
        }

        private void Start()
        {
            // _player = ObjFactory.Create(_playerSettings.prefab, _playerSettings._playerSpawnPosition);
            // _enemy = ObjFactory.Create(_enemySettings.prefab, _enemySettings._enemySpawnPosition);

            _projectilePool = new CustomPool<Projectile>(_projectileSettings.prefab, 10, transform, _resolver, true);
        }

        public void SpawnUnits()
        {
            SpawnPlayer();
            SpawnEnemy();
        }

        private void SpawnPlayer()
        {
            throw new NotImplementedException();
        }

        private void SpawnEnemy()
        {
            throw new NotImplementedException();
        }

        public void DespawnUnits()
        {
            throw new System.NotImplementedException();
        }

        public void SpawnProjectile()
        {
            throw new System.NotImplementedException();
        }
    }
}
