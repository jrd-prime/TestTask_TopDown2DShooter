using Game.Scripts.Help;
using UnityEngine;

namespace Game.Scripts.Settings.Main
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = AssetMenuConstants.GameSettings, order = 0)]
    public class GameSettings : InGameSettings
    {
        public int pointsPerKill = 10;
    }
}
