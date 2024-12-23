using System;
using Game.Scripts.Factory;
using Game.Scripts.Managers;
using Game.Scripts.PhysicsObjs.Enemy;
using Game.Scripts.PhysicsObjs.Player;
using Game.Scripts.Settings;
using Game.Scripts.Systems;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Scripts.Scope
{
    public class GameScope : LifetimeScope
    {
        [SerializeField] private SpawnManager spawnManager;

        protected override void Configure(IContainerBuilder builder)
        {
            Debug.LogWarning("Configure GameScope");
            var mainSettings = Parent.Container.Resolve<MainSettings>();

            // Player
            var playerSettings = mainSettings.playerSettings;
            var player = ObjFactory.Create(playerSettings.prefab, playerSettings.playerSpawnPosition);
            builder.RegisterComponent(player).As<IPlayer>();
            // Enemy
            var enemySettings = mainSettings.enemySettings;
            var enemy = ObjFactory.Create(enemySettings.prefab, enemySettings.enemySpawnPosition);
            builder.RegisterComponent(enemy).As<IEnemy>();


            builder.RegisterComponent(spawnManager).As<ISpawnManager>();


            builder.Register<PlayerMovementSystem>(Lifetime.Singleton)
                .As<IFixedTickable, IInitializable, IDisposable>();
        }
    }
}
