using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetsManager : MonoBehaviour
{
    public static TargetsManager _instance;
    
    [SerializeField]
    List<TargetCombat> enemys;
    [SerializeField]
    List<TargetCombat> players;

    List<TargetCombat> currentList;
    int currentTarget;

    private void Awake()
    {
        _instance = this;

        foreach(TargetCombat target in enemys) {
            target.OnSelectForTarget(false);
        }
        foreach (TargetCombat target in players) {
            target.OnSelectForTarget(false);
        }

        this.enabled = false;
    }

    List<TargetCombat> GetListTarget(bool isPlayer)
    {
        return isPlayer ? players : enemys;
    } 

    void OnEnable()
    {
        currentList = GetListTarget(!ActionsManager._instance.isAttack());

        if (ObjetsManager._instance.gameObject.activeInHierarchy)
            ObjetsManager._instance.enabled = false;
        else
            ActionsManager._instance.enabled = false;

        currentTarget = currentList.Count-1;
        SelectNextTarget(1);
    }

    public void OnClose()
    {
        InfoCombat._instance.gameObject.SetActive(false);
        this.enabled = false;
        currentList[currentTarget].OnSelectForTarget(false);

        if(ObjetsManager._instance.gameObject.activeInHierarchy)
            ObjetsManager._instance.enabled = true;
        else
            ActionsManager._instance.enabled = true;
    }

    private void Update()
    {
        int dir = InputController.downDown ? 1 : InputController.upDown ? -1 : 0;

        if (dir != 0) {
            SelectNextTarget(dir);
        }

        if (InputController.ineraction) {
            OnClose();

            if (ObjetsManager._instance.gameObject.activeInHierarchy)
                ObjetsManager._instance.OnGetTarget(currentList[currentTarget]);
            else
                ActionsManager._instance.OnGetTarget(currentList[currentTarget]);
        }

        if (InputController.exit) {
            MusicControler._instance.AudioClose();
            OnClose();
        }
    }

    public TargetCombat GetNewAliveTarget(TargetCombat target)
    {
        List<TargetCombat> targets = GetListTarget(target is PlayerCombat);

        int targetId = targets.IndexOf(target);
        int newId = mod(targetId + 1, targets.Count);
        while(targets[newId].isDead() && newId != targetId) {
            newId = mod(newId + 1, targets.Count);
        }

        return targets[newId];
    }

    void SelectNextTarget(int dir)
    {
        int newTarget = mod(currentTarget + dir, currentList.Count);

        int secureI = 0;
        while (currentList[newTarget].isDead() && secureI < 5) {
            newTarget = mod(newTarget + dir, currentList.Count);
            secureI++;
        }

        currentList[currentTarget].OnSelectForTarget(false);
        currentList[newTarget].OnSelectForTarget(true);
        currentTarget = newTarget;
    }

    public static int mod(int x, int m)
    {
        return (x % m + m) % m;
    }

    public void OnTargetDie(TargetCombat targetCombat, bool isPlayer)
    {
        List<TargetCombat> targets = GetListTarget(isPlayer);

        bool stillAlive = false;
        for (int i = 0; i < targets.Count; ++i) {
            if (!targets[i].isDead()) {
                stillAlive = true;
                break;
            }
        }

        if (!stillAlive) {
            CombatManager._instance.EndCombat(!isPlayer);
        } else if (this.enabled && currentList == targets && targets.IndexOf(targetCombat) == currentTarget) {
            SelectNextTarget(1);
        }
    }

    public TargetCombat GetRandomTarget(bool targetPlayer)
    {
        List<TargetCombat> targets = GetListTarget(targetPlayer);
        return targets[UnityEngine.Random.Range(0, targets.Count)];
    }

    public void AddEnemy(TargetCombat targetCombat)
    {
        targetCombat.InitCombat();
        enemys.Add(targetCombat);
    }

    public void CleanEnemys()
    {
        enemys = new List<TargetCombat>();
    }
}
