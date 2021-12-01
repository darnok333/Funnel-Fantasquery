using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjetCombat : ActionCombat
{
    public void SetName(int count)
    {
        _Name.text = name + "x" + count;
    }
}
