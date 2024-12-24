using System;
using Game.Scripts.Input;
using Game.Scripts.Managers;
using Game.Scripts.PhysicsObjs.Player;
using Game.Scripts.Settings;
using JetBrains.Annotations;
using R3;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Scripts.Systems
{
    [UsedImplicitly]
    public sealed class PlayerMovementSystem : IInitializable, IFixedTickable, IDisposable
    {
        private IUserInput _input;
        private readonly CompositeDisposable _disposable = new();
        private IPlayer _player;
        private Vector2 _direction;
        private PlayerSettings _playerSettings;
        private float _moveSpeed;
        private Vector2 _mousePosition;

        [Inject]
        private void Construct(IUserInput input, ISettingsManager settingsManager, GameManager gameManager)
        {
            _input = input;
            _playerSettings = settingsManager.GetSettings<PlayerSettings>();
            _player = gameManager.player;
        }

        public void Initialize()
        {
            if (_playerSettings == null) throw new Exception("PlayerSettings is null");
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
            var faceDir = _mousePosition - _player.GetPosition();
            var angle = Mathf.Atan2(faceDir.y, faceDir.x) * Mathf.Rad2Deg;
            _player.Rotate(angle);
        }

        private void Move()
        {
            _player.Move(_direction * _moveSpeed * 3f);
        }

        private void SetDirection(Vector2 direction) => _direction = direction;
        private void SetMousePosition(Vector2 mousePosition) => _mousePosition = mousePosition;


        public void Dispose() => _disposable?.Dispose();
    }
}
