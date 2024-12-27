using System;
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
        public ICharacter player { get; }
        public IEnemyCharacter enemy { get; }
        public void DespawnAll();
        public void Init(Action<ICharacter> gameManagerCallback);
    }

    public class SpawnManager : MonoBehaviour, ISpawnManager
    {
        public ICharacter player { get; private set; }
        public IEnemyCharacter enemy { get; private set; }

        private IObjectResolver _resolver;
        private ISettingsManager _settingsManager;
        private PlayerSettings _playerSettings;
        private EnemySettings _enemySettings;
        private ProjectileSettings _projectileSettings;

        private CustomPool<Projectile> _projectilesPool;
        private Projectile _projectile;
        private readonly List<ICharacter> _units = new();
        private Action<ICharacter> _onDeathCallback;
        private ObjFactory _factory;

        [Inject]
        private void Construct(IObjectResolver resolver)
        {
            _resolver = resolver;
            _settingsManager = resolver.Resolve<ISettingsManager>();
            _playerSettings = _settingsManager.GetSettings<PlayerSettings>();
            _enemySettings = _settingsManager.GetSettings<EnemySettings>();
            _projectileSettings = _settingsManager.GetSettings<ProjectileSettings>();
            _factory = resolver.Resolve<ObjFactory>();

            var holder = new GameObject("ProjectilesHolder");

            _projectilesPool =
                new CustomPool<Projectile>(_projectileSettings.prefab, 30, holder.transform, _resolver, true);
            player = _factory.CreateAndInject(_playerSettings.prefab, _playerSettings.playerSpawnPosition);
            enemy = _factory.CreateAndInject(_enemySettings.prefab, _enemySettings.enemySpawnPosition);

            _units.Add(player);
            _units.Add(enemy);
        }

        public void Init(Action<ICharacter> gameManagerCallback)
        {
            _onDeathCallback = gameManagerCallback;

            var playerWeapon = new PlayerWeapon(_projectilesPool);
            player.Initialize(_playerSettings.playerSpawnPosition, playerWeapon, _onDeathCallback);

            var enemyWeapon = new EnemyWeapon(_projectilesPool);
            enemy.Initialize(_enemySettings.enemySpawnPosition, enemyWeapon, _onDeathCallback);
        }

        public void DespawnAll()
        {
            _projectilesPool.ReturnAll();
            foreach (var unit in _units)
            {
                unit.Deactivate();
                unit.IsAlive = false;
            }
        }
    }
}
