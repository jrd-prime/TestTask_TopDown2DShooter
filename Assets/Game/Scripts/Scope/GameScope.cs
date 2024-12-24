using System;
using Game.Scripts.Managers;
using Game.Scripts.Systems;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Scripts.Scope
{
    public class GameScope : LifetimeScope
    {
        [SerializeField] private GameManager gameManager;
        [SerializeField] private SpawnManager spawnManager;

        protected override void Configure(IContainerBuilder builder)
        {
            Debug.LogWarning("Configure GameScope");

            builder.RegisterComponent(gameManager).AsSelf();
            builder.RegisterComponent(spawnManager).As<ISpawnManager>();

            builder.Register<PlayerMovementSystem>(Lifetime.Singleton)
                .As<IFixedTickable, IInitializable, IDisposable>();
            
        }
    }
}
