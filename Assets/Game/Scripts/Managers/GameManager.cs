using Game.Scripts.PhysicsObjs.Enemy;
using Game.Scripts.PhysicsObjs.Player;
using Game.Scripts.PhysicsObjs.Projectile;
using UnityEngine;
using VContainer;

namespace Game.Scripts.Managers
{
    public sealed class GameManager : MonoBehaviour
    {
        private ISpawnManager _spawnManager;
        private ISettingsManager _settingsManager;

        [Inject]
        private void Construct(ISpawnManager spawnManager, ISettingsManager settingsManager)
        {
            _spawnManager = spawnManager;
            _settingsManager = settingsManager;
        }

        public void StartGame()
        {
        }
    }
}
