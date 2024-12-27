using System;
using Game.Scripts.UI;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;

namespace Game.Scripts.Managers
{
    public interface IUIManager
    {
    }

    /* Скорее его стоит назвать GameplayUIManager, но тут и так достаточно, учитывая что тут один юай
     А если было бы несколько юай, то название подошло бы и уже бы был более комплексный класс*/
    [RequireComponent(typeof(UIDocument))]
    public sealed class UIManager : MonoBehaviour, IUIManager
    {
        [SerializeField] private PlayerPointsUI playerPointsUI;
        [SerializeField] private EnemyPointsUI enemyPointsUI;
        private IPointsUIModel _pointsUIModel;
        private VisualElement _root;

        [Inject]
        private void Construct(IPointsUIModel pointsUIModel)
        {
            _pointsUIModel = pointsUIModel;
        }

        private void Awake()
        {
            _root = GetComponent<UIDocument>().rootVisualElement ?? throw new Exception("Root Visual Element is null");
        }

        private void Start()
        {
            var views = new IView[] { playerPointsUI, enemyPointsUI };
            InitializeViews(views, _root);
        }

        private void InitializeViews(IView[] views, VisualElement rootVisualElement)
        {
            foreach (var view in views) view.Initialize(_pointsUIModel, rootVisualElement);
        }
    }
}
