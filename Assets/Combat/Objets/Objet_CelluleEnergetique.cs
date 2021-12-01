using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Objet_CelluleEnergetique : ObjetCombat
{
    public override void Execute(TargetCombat actor, TargetCombat target)
    {
        target.GiveMana((int)(damage * UnityEngine.Random.Range(0.75f, 1.25f)));
    }
}
