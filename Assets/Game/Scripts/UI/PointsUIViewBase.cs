using System;
using R3;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Scripts.UI
{
    public interface IView
    {
        public void Initialize(IPointsUIModel pointsUIModel, VisualElement rootVisualElement);
    }

    public abstract class PointsUIViewBase : MonoBehaviour, IView
    {
        [SerializeField] private string playerName;

        protected readonly CompositeDisposable Disposables = new();
        private Label nameLabel => GetNameLabel();
        private Label pointsLabel => GetPointsLabel();
        protected VisualElement root { get; private set; }
        protected IPointsUIModel model { get; private set; }

        public void Initialize(IPointsUIModel pointsUIModel, VisualElement rootVisualElement)
        {
            model = pointsUIModel;
            root = rootVisualElement ?? throw new ArgumentNullException(nameof(rootVisualElement));
            
            nameLabel.text = playerName.ToUpper();

            Subscribe();
        }

        protected abstract Label GetNameLabel();
        protected abstract Label GetPointsLabel();
        protected void UpdatePoints(int points) => pointsLabel.text = points.ToString();

        protected abstract void Subscribe();

        private void OnDestroy() => Disposables.Dispose();
    }
}
