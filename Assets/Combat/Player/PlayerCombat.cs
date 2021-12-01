using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCombat : TargetCombat
{
    [SerializeField]
    Transform RootBidule;
    [SerializeField]
    ATB_Barre ATB_Barre;
    [SerializeField]
    GameObject _SelectionAction;
    [SerializeField]
    Text _TextMaxLife, _TextCurrentLife;
    [SerializeField]
    Text _TextMaxMana, _TextCurrentMana;

    bool divideZero = false;
    int idRemovedAction = -1;

    public override bool isDivided()
    {
        return divideZero;
    }
    public override bool isInfiniteLoop()
    {
        return ATB_Barre.speed == 0;
    }
    public override bool isRemoved()
    {
        return idRemovedAction != -1;
    }

    public override void AddBidule(GameObject prefab)
    {
        Instantiate(prefab, RootBidule, false);
    }

    public override void RemoveBidules()
    {
        Transform[] tmp = RootBidule.GetComponentsInChildren<Transform>();
        for(int i = 0; i < tmp.Length; ++i) {
            if (RootBidule != tmp[i])
                Destroy(tmp[i].gameObject);
        }
    }

    public override void OnDivideZero(bool b)
    {
        divideZero = b;
        if(b)
            _TextCurrentLife.text = "NaN";
        else
            _TextCurrentLife.text = currentLife.ToString();
    }

    public override void OnRemoveAction(bool b)
    {
        if (b)
            idRemovedAction = UnityEngine.Random.Range(0, _Actions.Count);
        else
            idRemovedAction = -1;
    }

    internal int GetMana()
    {
        return currentMana;
    }

    public override void OnInfiniteLoop(bool b)
    {
        if (b)
            ATB_Barre.speed = 0;
        else
            ATB_Barre.ResetSpeed();
    }

    public override void InitCombat()
    {
        base.InitCombat();
        animatorSprite.SetBool("win", false);
        _SelectionAction.SetActive(false);
        _TextMaxLife.text = " - " + MaxLife.ToString();
        _TextMaxMana.text = " - " + MaxMana.ToString();
        if (!divideZero)
            _TextCurrentLife.text = currentLife.ToString();
        _TextCurrentMana.text = currentMana.ToString();
        ATB_Barre.InitCombat();
    }

    public void OnSelectForAction(bool b)
    {
        _SelectionAction.SetActive(b);

        if (b)
            MusicControler._instance.AudioSelect();
    }

    public void OnOpenActionList()
    {
        ActionsManager._instance.OnOpen(_Actions, idRemovedAction);
    }

    public override void OnEndAction()
    {
        if (CombatManager._instance.isWin())
            return;

        base.OnEndAction();
        ATB_Barre.Restart();
    }

    public override void Hit(int damage)
    {
        base.Hit(damage);
        if (!divideZero)
            _TextCurrentLife.text = currentLife.ToString();
    }

    public override void Heal(int damage)
    {
        base.Heal(damage);
        if (!divideZero)
            _TextCurrentLife.text = currentLife.ToString();
    }

    public override void GiveMana(int damage)
    {
        base.GiveMana(damage);
        _TextCurrentMana.text = currentMana.ToString();
    }

    public override void UseMana(int manaCost)
    {
        base.UseMana(manaCost);
        _TextCurrentMana.text = currentMana.ToString();
    }

    protected override void Die()
    {
        base.Die();

        ATB_Barre.gameObject.SetActive(false);
        PlayersManager._instance.OnPlayerDie(this);
        animatorPos.SetBool("die", true);
    }

    public void OnWin()
    {
        animatorSprite.SetBool("win", !isDead());
    }
}
