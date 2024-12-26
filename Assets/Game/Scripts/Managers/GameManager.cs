using System;
using System.Threading.Tasks;
using Game.Scripts.Factory;
using Game.Scripts.Input;
using Game.Scripts.PhysicsObjs.Character;
using Game.Scripts.PhysicsObjs.Character.Enemy;
using Game.Scripts.Systems.Projectile;
using R3;
using UnityEngine;
using VContainer;

namespace Game.Scripts.Managers
{
    public interface IGameManager
    {
        public ICharacter player { get; }
        public IEnemyCharacter enemy { get; }
        public bool IsRoundOver { get; }
    }

    public sealed class GameManager : MonoBehaviour, IGameManager
    {
        public ICharacter player { get; private set; }
        public IEnemyCharacter enemy { get; private set; }
        public bool IsRoundOver { get; private set; }

        private ISpawnManager _spawnManager;
        private ISettingsManager _settingsManager;
        private IObjectResolver _resolver;
        private IUserInput _input;
        private IPointsManager _po;

        private ObjFactory _factory;
        private readonly CompositeDisposable _disposable = new();
        private HitHandlerSystem _hit;

        [Inject]
        private void Construct(IObjectResolver resolver)
        {
            _resolver = resolver;
            _po = resolver.Resolve<IPointsManager>();
            _input = resolver.Resolve<IUserInput>();
            _spawnManager = resolver.Resolve<ISpawnManager>();
            _settingsManager = resolver.Resolve<ISettingsManager>();
            _hit = resolver.Resolve<HitHandlerSystem>();
        }

        private void Start()
        {
            player = _spawnManager.player ?? throw new Exception("Player is null");
            enemy = _spawnManager.enemy ?? throw new Exception("Enemy is null");
            if (_input == null) throw new Exception("Input is null");

            _input.Shoot.Subscribe(_ => player.Fire()).AddTo(_disposable);

            _hit.someTargetKilled.Subscribe(RoundOver).AddTo(_disposable);

            StartGame();
        }

        private async void RoundOver(Unit _)
        {
            Debug.LogWarning("RoundOver");

            IsRoundOver = true;
            _spawnManager.DespawnAll();

            await Task.Delay(2000);

            RestartGame();
        }

        private void RestartGame()
        {
            Debug.LogWarning("RestartGame");
            _spawnManager.SpawnUnits();
            _spawnManager.ResetUnits();
        }

        private void StartGame()
        {
            _spawnManager.SpawnUnits();
        }

        private void OnDestroy() => _disposable.Dispose();
    }
}
