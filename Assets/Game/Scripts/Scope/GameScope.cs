using System;
using Game.Scripts.Managers;
using Game.Scripts.Systems;
using Game.Scripts.Systems.Enemy;
using Game.Scripts.Systems.Player;
using Game.Scripts.Systems.Projectile;
using Game.Scripts.UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Scripts.Scope
{
    public class GameScope : LifetimeScope
    {
        [SerializeField] private GameManager gameManager;
        [SerializeField] private SpawnManager spawnManager;
        [SerializeField] private UIManager uiManager;

        protected override void Configure(IContainerBuilder builder)
        {
            Debug.LogWarning("Configure GameScope");

            if (gameManager == null)
                throw new MissingReferenceException($"You must add {nameof(gameManager)} to {nameof(GameScope)}");
            if (spawnManager == null)
                throw new MissingReferenceException($"You must add {nameof(spawnManager)} to {nameof(GameScope)}");
            if (uiManager == null)
                throw new MissingReferenceException($"You must add {nameof(uiManager)} to {nameof(GameScope)}");

            builder.RegisterComponent(gameManager).As<IGameManager>();
            builder.RegisterComponent(spawnManager).As<ISpawnManager>();
            builder.RegisterComponent(uiManager).As<IUIManager>();

            builder.Register<PlayerMovementSystem>(Lifetime.Singleton)
                .As<IFixedTickable, IInitializable, IDisposable>();

            builder.Register<IPointsManager, PointsManager>(Lifetime.Singleton).As<IDisposable>();
            builder.Register<IPointsUIModel, PointsUIModel>(Lifetime.Singleton).As<IInitializable, IDisposable>();


            builder.Register<OutOfBoundsCheckSystem>(Lifetime.Singleton).AsSelf();
            builder.Register<HitHandlerSystem>(Lifetime.Singleton).AsSelf();

            builder.Register<EnemyMovementSystem>(Lifetime.Singleton).AsSelf().As<IInitializable>();
            builder.Register<EnemyFollowSystem>(Lifetime.Singleton).AsSelf()
                .As<IInitializable, IFixedTickable, IPostStartable>();
            builder.Register<EnemyShootSystem>(Lifetime.Singleton).AsSelf()
                .As<IInitializable, IFixedTickable, IPostStartable>();
        }
    }
}
