using System;
using Game.Scripts.PhysicsObjs.Character;
using Game.Scripts.Settings.Main;
using JetBrains.Annotations;
using R3;
using VContainer;

namespace Game.Scripts.Managers
{
    public interface IPointsManager : IDisposable
    {
        public ReactiveProperty<int> playerPoints { get; }
        public ReactiveProperty<int> enemyPoints { get; }
        public void AddPoints(CharType to);
        public void ResetPoints();
        public void AddPointsOnTargetDeath(ICharacter character);
    }

    [UsedImplicitly]
    public sealed class PointsManager : IPointsManager
    {
        public ReactiveProperty<int> playerPoints { get; } = new(0);
        public ReactiveProperty<int> enemyPoints { get; } = new(0);

        private GameSettings _gameSettings;

        [Inject]
        private void Construct(ISettingsManager settingsManager)
        {
            _gameSettings = settingsManager.GetSettings<GameSettings>();
        }

        public void AddPoints(CharType to)
        {
            var pointsPerKill = _gameSettings.pointsPerKill;
            switch (to)
            {
                case CharType.NotSet:
                    break;
                case CharType.Player:
                    playerPoints.Value += pointsPerKill;
                    break;
                case CharType.Enemy:
                    enemyPoints.Value += pointsPerKill;
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

        public void AddPointsOnTargetDeath(ICharacter character)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            playerPoints?.Dispose();
            enemyPoints?.Dispose();
        }
    }
}
