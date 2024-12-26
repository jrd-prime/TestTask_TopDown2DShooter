using System;
using Game.Scripts.Help;
using R3;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Scripts.UI
{
    public class EnemyPointsUI : PointsUIViewBase
    {
        protected override Label GetNameLabel() => root.Q<Label>(UINameID.PointsUIEnemyNameLabelID) ??
                                                   throw new Exception("NameLabel is null");

        protected override Label GetPointsLabel() => root.Q<Label>(UINameID.PointsUIEnemyPointsLabelID) ??
                                                     throw new Exception("PointsLabel is null");

        protected override void Subscribe()
        {
            Debug.LogWarning("enemyPoints.Subscribe");
            model.enemyPoints.Subscribe(UpdatePoints).AddTo(Disposables);
        }
    }
}
