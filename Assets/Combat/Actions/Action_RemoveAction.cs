using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Action_RemoveAction : ActionCombat
{
    [SerializeField]
    GameObject _Prefab;

    public override void Execute(TargetCombat actor, TargetCombat target)
    {
        if (target.isRemoved())
            return;

        target.OnRemoveAction(true);
        target.AddBidule(_Prefab);
    }
}
