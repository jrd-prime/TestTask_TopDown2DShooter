using System;
using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Settings;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Scripts.Managers
{
    public interface ISettingsManager : IInitializable
    {
        public T GetSettings<T>() where T : InGameSettings;
    }

    public class SettingsManager : ISettingsManager
    {
        private Dictionary<Type, InGameSettings> _settingsCache = new();

        [Inject]
        private void Construct(MainSettings mainSettings)
        {
            Debug.LogWarning("SettingsManager Construct");
            _settingsCache = mainSettings.settingsDictionary;
            Debug.LogWarning(_settingsCache.Count);
        }

        public void Initialize()
        {
        }

        public T GetSettings<T>() where T : InGameSettings
        {
            if (!_settingsCache.ContainsKey(typeof(T)))
                throw new ArgumentException($"Settings of {nameof(T)} is not registered in {nameof(SettingsManager)}");

            return (T)_settingsCache[typeof(T)];
        }
    }
}
