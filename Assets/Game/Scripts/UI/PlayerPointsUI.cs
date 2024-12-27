using System;
using Game.Scripts.Help;
using R3;
using UnityEngine.UIElements;

namespace Game.Scripts.UI
{
    public class PlayerPointsUI : PointsUIViewBase
    {
        protected override Label GetNameLabel() => root.Q<Label>(UINameID.PointsUIPlayerNameLabelID) ??
                                                   throw new Exception("NameLabel is null");

        protected override Label GetPointsLabel() => root.Q<Label>(UINameID.PointsUIPlayerPointsLabelID) ??
                                                     throw new Exception("PointsLabel is null");

        protected override void Subscribe()
        {
            model.playerPoints.Subscribe(UpdatePoints).AddTo(Disposables);
        }
    }
}
