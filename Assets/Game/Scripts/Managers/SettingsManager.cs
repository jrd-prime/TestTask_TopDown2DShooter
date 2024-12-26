using System;
using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Settings;
using Game.Scripts.Settings.Main;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Scripts.Managers
{
    public interface ISettingsManager : IInitializable, IDisposable
    {
        public T GetSettings<T>() where T : InGameSettings;
    }

    public class SettingsManager : ISettingsManager
    {
        private Dictionary<Type, InGameSettings> _settingsCache = new();

        [Inject]
        private void Construct(MainSettings mainSettings)
        {
            _settingsCache = mainSettings.GetSettingsList();
        }

        public void Initialize()
        {
            if (_settingsCache.Count == 0) throw new Exception("No settings registered");
        }

        public T GetSettings<T>() where T : InGameSettings
        {
            if (!_settingsCache.ContainsKey(typeof(T)))
                throw new ArgumentException($"Settings of {nameof(T)} is not registered in {nameof(SettingsManager)}");

            return (T)_settingsCache[typeof(T)];
        }

        public void Dispose()
        {
            _settingsCache.Clear();
        }
    }
}
