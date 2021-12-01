using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Action_Objet : ActionCombat
{
    public override void OnChooseAction()
    {
        ObjetsManager._instance.OnOpen();
    }
}
