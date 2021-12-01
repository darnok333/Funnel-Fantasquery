using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Bug_Action
{
    Attack,
    Attack_Enemy,
    RemoveAction,
    DivideZero,
    UniverseWithin,
    InfiniteLoop,
    AttackLosange,
    AttackMinus,
    AttackOval,
    AttackTriangle,
    Attack_EnemyBig,
    TwistedToroid
}

public class ActionsManager : MonoBehaviour
{
    public static ActionsManager _instance;
    
    [SerializeField]
    List<ActionCombat> listActions;
    [SerializeField]
    ActionCombat actionObjet;
    List<ActionCombat> actions;

    int currentAction;

    private void Awake()
    {
        _instance = this;

        enabled = true;
        gameObject.SetActive(false);
    }

    public void SetNames(bool random)
    {
        for (int i = 0; i < listActions.Count; ++i) {
            listActions[i].SetName(random);
        }
        actionObjet.SetName(random);
    }

    public void OnOpen(List<Bug_Action> listActionsID, int idRemovedAction)
    {
        actions = new List<ActionCombat>();
        for(int i = 0; i < listActions.Count; ++i) {
            if (listActionsID.Contains((Bug_Action)i)) {
                listActions[i].gameObject.SetActive(true);
                listActions[i].OnSelect(false);
                listActions[i].SetName(idRemovedAction == i);
                listActions[i].SetColor(listActions[i].manaCost <= PlayersManager._instance.GetCurrentMana());
                if (idRemovedAction != i && listActions[i].manaCost <= PlayersManager._instance.GetCurrentMana())
                    actions.Add(listActions[i]);
            } else {
                listActions[i].gameObject.SetActive(false);
            }
        }
        actions.Add(actionObjet);

        gameObject.SetActive(true);
        PlayersManager._instance.enabled = false;
        currentAction = 0;
        actions[currentAction].OnSelect(true);
    }

    public ActionCombat GetRandomAction(List<Bug_Action> listActionsID)
    {
        List<ActionCombat> actionsTmp = new List<ActionCombat>();
        for (int i = 0; i < listActions.Count; ++i) {
            if (listActionsID.Contains((Bug_Action)i)) {
                actionsTmp.Add(listActions[i]);
            }
        }

        return actionsTmp[UnityEngine.Random.Range(0, actionsTmp.Count)];
    }

    public void OnClose()
    {
        InfoCombat._instance.gameObject.SetActive(false);
        actions[currentAction].OnSelect(false);
        PlayersManager._instance.enabled = true;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        int dir = InputController.downDown ? 1 : InputController.upDown ? -1 : 0;

        if (dir != 0) {
            SelectNextAction(dir);
        }

        if (InputController.ineraction) {
            actions[currentAction].OnChooseAction();
        }

        if (InputController.exit) {
            MusicControler._instance.AudioClose();
            OnClose();
        }
    }

    public bool isAttack()
    {
        return actions[currentAction].isAttack();
    }

    void SelectNextAction(int dir)
    {
        int newAction = mod(currentAction + dir, actions.Count);

        actions[currentAction].OnSelect(false);

        actions[newAction].OnSelect(true);
        currentAction = newAction;
    }

    public static int mod(int x, int m)
    {
        return (x % m + m) % m;
    }

    public void OnGetTarget(TargetCombat targetCombat, ObjetCombat actionCombat = null)
    {
        OnClose();
        MusicControler._instance.AudioOpen();
        PlayersManager._instance.OnAskForAction(actionCombat != null ? actionCombat : actions[currentAction], targetCombat);
    }
}
