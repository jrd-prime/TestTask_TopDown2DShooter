using Game.Scripts.Factory;
using Game.Scripts.Input;
using Game.Scripts.Managers;
using Game.Scripts.Settings;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Scripts.Scope
{
    public class RootScope : LifetimeScope
    {
        [SerializeField] private MainSettings mainSettings;

        protected override void Configure(IContainerBuilder builder)
        {
            Debug.LogWarning("Configure RootScope");

            if (mainSettings == null)
                throw new MissingReferenceException($"You must add {nameof(mainSettings)} to {nameof(RootScope)}");
            builder.RegisterComponent(mainSettings);

            builder.Register<ISettingsManager, SettingsManager>(Lifetime.Singleton).As<IInitializable>();


            var input = gameObject.AddComponent(typeof(PCUserInput));
            builder.RegisterComponent(input).As<IUserInput>();

            builder.Register<ObjFactory>(Lifetime.Singleton);
        }
    }
}
