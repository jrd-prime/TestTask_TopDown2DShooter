using System;
using Game.Scripts.Settings;
using Game.Scripts.Settings.Main;
using Game.Scripts.Systems;
using Game.Scripts.Systems.Projectile;
using JetBrains.Annotations;
using R3;
using VContainer;

namespace Game.Scripts.Managers
{
    public interface IPointsManager : IDisposable
    {
        public ReactiveProperty<int> playerPoints { get; }
        public ReactiveProperty<int> enemyPoints { get; }
        public void AddPoints(CharacterType to);
        public void ResetPoints();
    }

    [UsedImplicitly]
    public sealed class PointsManager : IPointsManager
    {
        public ReactiveProperty<int> playerPoints { get; } = new(0);
        public ReactiveProperty<int> enemyPoints { get; } = new(0);
        private ISettingsManager _settingsManager;
        private GameSettings _gameSettings;

        [Inject]
        private void Construct(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
            _gameSettings = settingsManager.GetSettings<GameSettings>();
        }

        public void AddPoints(CharacterType to)
        {
            var pointsPerKill = _gameSettings.pointsPerKill;
            switch (to)
            {
                case CharacterType.NotSet:
                    break;
                case CharacterType.Player:
                    enemyPoints.Value += pointsPerKill;
                    break;
                case CharacterType.Enemy:
                    playerPoints.Value += pointsPerKill;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(to), to, null);
            }
        }

        public void ResetPoints()
        {
            playerPoints.Value = 0;
            enemyPoints.Value = 0;
        }

        public void Dispose()
        {
            playerPoints?.Dispose();
            enemyPoints?.Dispose();
        }
    }
}
