using Game.Scripts.Managers;
using Game.Scripts.PhysicsObjs.Character;
using R3;
using VContainer;

namespace Game.Scripts.Systems.Projectile
{
    public enum CharacterType
    {
        NotSet,
        Player,
        Enemy
    }

    public sealed class HitHandlerSystem
    {
        private IPointsManager _pointsManager;
        public Subject<Unit> SomeTargetKilled { get; } = new();

        [Inject]
        private void Construct(IPointsManager pointsManager) => _pointsManager = pointsManager;

        public void HitTarget(CharacterBase go)
        {
            var to = go switch
            {
                PhysicsObjs.Character.Player.Player => CharacterType.Player,
                PhysicsObjs.Character.Enemy.Enemy => CharacterType.Enemy,
                _ => default
            };

            if (to == CharacterType.NotSet) return;

            _pointsManager.AddPoints(to);
            SomeTargetKilled.OnNext(Unit.Default);
        }
    }
}
