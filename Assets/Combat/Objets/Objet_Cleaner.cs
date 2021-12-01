using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Objet_Cleaner : ObjetCombat
{
    public override void Execute(TargetCombat actor, TargetCombat target)
    {
        target.OnDivideZero(false);
        target.OnRemoveAction(false);
        target.OnInfiniteLoop(false);
        target.RemoveBidules();
    }
}
