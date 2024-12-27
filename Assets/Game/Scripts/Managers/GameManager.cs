using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Game.Scripts.Factory;
using Game.Scripts.Input;
using Game.Scripts.PhysicsObjs.Character;
using Game.Scripts.PhysicsObjs.Character.Enemy;
using Game.Scripts.PhysicsObjs.Character.Player;
using R3;
using UnityEngine;
using VContainer;

namespace Game.Scripts.Managers
{
    public interface IGameManager
    {
        public ICharacter player { get; }
        public IEnemyCharacter enemy { get; }
    }

    public sealed class GameManager : MonoBehaviour, IGameManager
    {
        public ICharacter player { get; private set; }
        public IEnemyCharacter enemy { get; private set; }

        private ISpawnManager _spawnManager;
        private IUserInput _input;
        private IPointsManager _po;

        private ObjFactory _factory;
        private Action<ICharacter> _gameManagerCallback;
        private readonly CompositeDisposable _disposable = new();
        private List<ICharacter> _units;


        private IGameLoop _gameLoop;
        private IUnitsManager _unitsManager;

        private const int GameRestartDelayMs = 1000;

        [Inject]
        private void Construct(IObjectResolver resolver)
        {
            _po = resolver.Resolve<IPointsManager>();
            _input = resolver.Resolve<IUserInput>();
            _spawnManager = resolver.Resolve<ISpawnManager>();


            _gameLoop = resolver.Resolve<IGameLoop>();
            _unitsManager = resolver.Resolve<IUnitsManager>();
        }

        private void Awake()
        {
            _gameManagerCallback += OnCharacterDeath;

            if (_input == null) throw new Exception("Input is null");
            _input.Shoot.Subscribe(OnFireBtnClick).AddTo(_disposable);
            _spawnManager.Init(_gameManagerCallback);

            player = _spawnManager.player ?? throw new Exception("Player is null");
            enemy = _spawnManager.enemy ?? throw new Exception("Enemy is null");

            _units = new List<ICharacter> { player, enemy };

            _gameLoop.SetUnits(_units);
        }

        private void Start()
        {
            _gameLoop.StartGame();
            StartGame();
        }

        private void AddPointsOnTargetDeath(ICharacter characterKilled)
        {
            CharType to;
            switch (characterKilled)
            {
                case Player: to = CharType.Enemy; break;
                case Enemy: to = CharType.Player; break;
                default: return;
            }

            _po.AddPoints(to);
        }

        private void OnCharacterDeath(ICharacter character)
        {
            _po.AddPointsOnTargetDeath(character);
            
            
            AddPointsOnTargetDeath(character);
            SetGameRunning(false);
            GameRestartAsync().GetAwaiter();
        }

        private async Task GameRestartAsync()
        {
            _spawnManager.DespawnAll();
            ActivateUnits();
            ResetUnits();

            await Task.Delay(GameRestartDelayMs);
            SetGameRunning(true);
        }

        private void ResetUnits()
        {
            foreach (var unit in _units) unit.ResetCharacter();
        }

        private void ActivateUnits()
        {
            foreach (var unit in _units)
            {
                unit.Activate();
                unit.IsAlive = true;
            }
        }

        private void SetGameRunning(bool b)
        {
            foreach (var unit in _units) unit.IsGameRunning = b;
        }

        private void StartGame()
        {
            SetGameRunning(true);
            ActivateUnits();
        }

        private void OnFireBtnClick(Unit _) => player.Fire();
        private void OnDestroy() => _disposable.Dispose();
    }

    public interface IUnitsManager
    {
    }

    public sealed class UnitsManager : IUnitsManager{}
}
