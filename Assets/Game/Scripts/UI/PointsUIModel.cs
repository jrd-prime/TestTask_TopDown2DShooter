using System;
using Game.Scripts.Managers;
using R3;
using VContainer;
using VContainer.Unity;

namespace Game.Scripts.UI
{
    public interface IPointsUIModel : IInitializable, IDisposable
    {
        public ReadOnlyReactiveProperty<int> playerPoints { get; }
        public ReadOnlyReactiveProperty<int> enemyPoints { get; }
    }

    public class PointsUIModel : IPointsUIModel
    {
        private IPointsManager _pointsManager;
        public ReadOnlyReactiveProperty<int> playerPoints => _pointsManager.playerPoints;
        public ReadOnlyReactiveProperty<int> enemyPoints => _pointsManager.enemyPoints;

        private readonly CompositeDisposable _disposable = new();

        [Inject]
        private void Construct(IPointsManager pointsManager)
        {
            _pointsManager = pointsManager;
        }

        public void Initialize()
        {
        }

        public void Dispose() => _disposable.Dispose();
    }
}
