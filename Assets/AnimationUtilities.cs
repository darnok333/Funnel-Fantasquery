using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationUtilities : MonoBehaviour
{
    public void Destroy()
    {
        Destroy(gameObject);
    }

    public void InstantiateParticle()
    {
        CombatManager._instance.InstantiateParticle();
    }

    public void ExecuteAction()
    {
        CombatManager._instance.ExecuteAction();
    }

    public void MoveBackCharacter()
    {
        CombatManager._instance.MoveBackCharacter();
    }
}
