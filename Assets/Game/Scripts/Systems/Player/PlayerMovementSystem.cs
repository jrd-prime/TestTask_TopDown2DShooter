using System;
using Game.Scripts.Help;
using Game.Scripts.Input;
using Game.Scripts.Managers;
using Game.Scripts.PhysicsObjs.Character;
using Game.Scripts.Settings;
using JetBrains.Annotations;
using R3;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Scripts.Systems.Player
{
    [UsedImplicitly]
    public sealed class PlayerMovementSystem : IInitializable, IFixedTickable, IDisposable
    {
        private IUserInput _input;
        private readonly CompositeDisposable _disposable = new();
        private ICharacter _player;
        private Vector2 _direction;
        private PlayerSettings _playerSettings;
        private float _moveSpeed;
        private Vector2 _mousePosition;

        [Inject]
        private void Construct(IUserInput input, ISettingsManager settingsManager, ISpawnManager spawnManager)
        {
            _input = input;
            _playerSettings = settingsManager.GetSettings<PlayerSettings>();
            _player = spawnManager.player;
        }

        public void Initialize()
        {
            if (_playerSettings == null) throw new Exception("PlayerSettings is null");
            if (_player == null) throw new Exception("Player is null");
            _moveSpeed = _playerSettings.playerSpeed;
            _input.MoveDirection.Subscribe(SetDirection).AddTo(_disposable);
            _input.MousePosition.Skip(1).Subscribe(SetMousePosition).AddTo(_disposable);
        }

        public void FixedTick()
        {
            Move();
            Rotate();
        }

        private void Rotate()
        {
            var angle = RotateAngle.GetAngle(_player.GetTransform().position, _mousePosition);
            _player.Rotate(angle);
        }

        private void Move()
        {
            _player.AddForce(_direction * _moveSpeed * 3f);
        }

        private void SetDirection(Vector2 direction) => _direction = direction;

        private void SetMousePosition(Vector2 mousePosition) => _mousePosition = mousePosition;

        public void Dispose() => _disposable?.Dispose();
    }
}
