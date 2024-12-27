using System.Collections.Generic;
using Game.Scripts.PhysicsObjs.Character;

namespace Game.Scripts.Managers
{
    public sealed class GameLoop : IGameLoop
    {
        private List<ICharacter> _units;

        public void StartGame()
        {
        }

        public void SetUnits(List<ICharacter> units) => _units = units;
    }

    public interface IGameLoop
    {
        public void StartGame();
        public void SetUnits(List<ICharacter> units);
    }
}
