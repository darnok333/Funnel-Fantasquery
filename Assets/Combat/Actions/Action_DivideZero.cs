using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Action_DivideZero : ActionCombat
{
    [SerializeField]
    GameObject _NaNPrefab;

    public override void Execute(TargetCombat actor, TargetCombat target)
    {
        if (target.isDivided())
            return;

        target.OnDivideZero(true);
        target.AddBidule(_NaNPrefab);
    }
}
