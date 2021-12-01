using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetCombat : MonoBehaviour
{
    [SerializeField]
    protected Animator animatorPos, animatorSprite;
    [SerializeField]
    protected List<Bug_Action> _Actions;

    [SerializeField]
    GameObject _SelectionTarget;
    [SerializeField]
    protected int MaxLife, MaxMana;
    public string info;

    public virtual void AddBidule(GameObject prefab)
    {
    }
    public virtual void RemoveBidules()
    {
    }

    protected int currentLife, currentMana;

    private void Awake()
    {
        currentLife = MaxLife;
        currentMana = MaxMana;
    }

    public virtual void InitCombat()
    {
        _SelectionTarget.SetActive(false);
        animatorPos.SetBool("moveFront", false);
        animatorSprite.ResetTrigger("attack");
        animatorSprite.ResetTrigger("move");
        animatorPos.SetTrigger("init");
        animatorSprite.SetTrigger("init");
    }

    public void OnSelectForTarget(bool b)
    {
        _SelectionTarget.SetActive(b);

        if (b) {
            InfoCombat._instance.SetInfo(info);
            MusicControler._instance.AudioSelect();
        }
    }

    public virtual void UseMana(int manaCost)
    {
        currentMana -= manaCost;
    }

    public virtual void Hit(int damage)
    {
        currentLife = Mathf.Max(0, currentLife - damage);
        CombatManager._instance.InstantiateDamagePrefab(transform.position, damage);

        if (currentLife == 0)
            Die();
    }

    public virtual void Heal(int damage)
    {
        currentLife = Mathf.Min(MaxLife, currentLife + damage);
        CombatManager._instance.InstantiateHealPrefab(transform.position, damage);
    }

    public virtual void GiveMana(int damage)
    {
        currentMana = Mathf.Min(MaxMana, currentMana + damage);
        CombatManager._instance.InstantiateManaPrefab(transform.position, damage);
    }

    protected virtual void Die()
    {
        TargetsManager._instance.OnTargetDie(this, this is PlayerCombat);
        animatorSprite.SetBool("dead", true);
    }

    public bool isDead()
    {
        return currentLife == 0;
    }


    //ANIMATION
    public void MoveFrontCharacter()
    {
        animatorPos.SetBool("moveFront", true);
        animatorSprite.SetTrigger("move");
    }

    public void AnimAttackCharacter()
    {
        animatorSprite.SetInteger("state", CombatManager._instance.GetAnimState());
        animatorSprite.SetTrigger("attack");
    }

    public void MoveBackCharacter()
    {
        animatorPos.SetBool("moveFront", false);
    }

    public virtual void OnEndAction()
    {
        CombatManager._instance.enabled = true;
    }

    private void OnDisable()
    {
    }



    public virtual void OnDivideZero(bool b)
    {
    }

    public virtual void OnRemoveAction(bool b)
    {
    }

    public virtual void OnInfiniteLoop(bool b)
    {
    }

    public virtual bool isDivided()
    {
        return false;
    }
    public virtual bool isInfiniteLoop()
    {
        return false;
    }
    public virtual bool isRemoved()
    {
        return false;
    }
}
