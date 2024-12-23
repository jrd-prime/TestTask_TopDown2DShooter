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
        private Player _player;
        private Vector2 _direction;
        private PlayerSettings _playerSettings;
        private float _moveSpeed;
        private Vector2 mousePos;
        private Rigidbody2D _rb;

        [Inject]
        private void Construct(IUserInput input, Player player, ISettingsManager settingsManager)
        {
            _input = input;
            _player = player;
            _playerSettings = settingsManager.GetSettings<PlayerSettings>();
        }

        public void Initialize()
        {
            if (_playerSettings == null) throw new Exception("PlayerSettings is null");
            _moveSpeed = _playerSettings.playerSpeed;

            _rb = _player.GetComponent<Rigidbody2D>();

            _input.MoveDirection.Subscribe(SetDirection).AddTo(_disposable);
            _input.MousePosition.Skip(1).Subscribe(CalcRotation).AddTo(_disposable);
        }

        public void FixedTick()
        {
            _rb.AddForce(_direction * _moveSpeed * 3f);

            var faceDir = mousePos - _rb.position;
            var angle = Mathf.Atan2(faceDir.y, faceDir.x) * Mathf.Rad2Deg;
            _rb.MoveRotation(angle);
        }

        private void SetDirection(Vector2 direction) => _direction = direction;

        private void CalcRotation(Vector2 mousePosition)
        {
            mousePos = mousePosition;
        }

        public void Dispose() => _disposable?.Dispose();
    }
}
