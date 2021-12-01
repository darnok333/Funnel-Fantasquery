using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Action_InfiniteLoop : ActionCombat
{
    [SerializeField]
    GameObject _Prefab;

    public override void Execute(TargetCombat actor, TargetCombat target)
    {
        if (target.isInfiniteLoop())
            return;

        target.OnInfiniteLoop(true);
        target.AddBidule(_Prefab);
    }
}
