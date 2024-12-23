using Game.Scripts.Weapon;

namespace Game.Scripts.PhysicsObjs.Player
{
    public interface IPlayer
    {
    }

    public class Player : PhysicsObject, IPlayer
    {
        private IWeapon _weapon;
    }
}
