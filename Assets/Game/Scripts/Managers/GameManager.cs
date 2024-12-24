using System;
using Game.Scripts.Factory;
using Game.Scripts.Input;
using Game.Scripts.PhysicsObjs.Enemy;
using Game.Scripts.PhysicsObjs.Player;
using Game.Scripts.PhysicsObjs.Projectile;
using Game.Scripts.Settings;
using Game.Scripts.Weapon;
using R3;
using Unity.VisualScripting;
using UnityEngine;
using VContainer;

namespace Game.Scripts.Managers
{
    public sealed class GameManager : MonoBehaviour
    {
        private ISpawnManager _spawnManager;
        private ISettingsManager _settingsManager;
        private IObjectResolver _resolver;
        private IUserInput _input;

        private ObjFactory _factory;

        private readonly CompositeDisposable _disposable = new();

        public IPlayer player { get; private set; }
        public Enemy enemy { get; private set; }
        public CustomPool<Projectile> projectilesPool { get; private set; }

        [Inject]
        private void Construct(ISpawnManager spawnManager, ISettingsManager settingsManager, IObjectResolver resolver)
        {
            _resolver = resolver;
            _input = resolver.Resolve<IUserInput>();
            _spawnManager = spawnManager;
            _settingsManager = settingsManager;
            _factory = resolver.Resolve<ObjFactory>();


            var playerSettings = _settingsManager.GetSettings<PlayerSettings>();
            player = _factory.CreateAndInject(playerSettings.prefab, playerSettings.playerSpawnPosition);

            var enemySettings = _settingsManager.GetSettings<EnemySettings>();
            enemy = _factory.CreateAndInject(enemySettings.prefab, enemySettings.enemySpawnPosition);

            var projectileSettings = _settingsManager.GetSettings<ProjectileSettings>();
            // var projectile = _factory.CreateAndInject(projectileSettings.prefab, null);

            projectilesPool = new CustomPool<Projectile>(projectileSettings.prefab, 30, transform, _resolver, true);


            var playerWeapon = new PlayerWeapon(projectilesPool);
            var enemyWeapon = new EnemyWeapon(projectilesPool);
            player.SetWeapon(playerWeapon);
            enemy.SetWeapon(enemyWeapon);


            if (_input == null) throw new Exception("Input is null");
            _input.Shoot.Subscribe(_ => player.Fire()).AddTo(_disposable);
        }

        private void Awake()
        {
            Debug.LogWarning($"Player {player}");
            Debug.LogWarning($"Enemy {enemy}");
        }

        public void StartGame()
        {
        }

        private void OnDestroy() => _disposable.Dispose();
    }
}
