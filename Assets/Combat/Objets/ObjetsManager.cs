using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Bug_Objet
{
    Repair,
    Cleaner,
    CelluleEnergetique
}

public class ObjetsManager : MonoBehaviour
{
    public static ObjetsManager _instance;
    
    [SerializeField]
    List<ObjetCombat> listObjets;
    List<ObjetCombat> objets;
    [SerializeField]
    List<int> objetsCount;

    int currentObjet;

    private void Awake()
    {
        _instance = this;

        enabled = true;
        gameObject.SetActive(false);

        objetsCount = new List<int>();
        for (int i = 0; i < listObjets.Count; ++i) {
            objetsCount.Add(0);
        }
    }

    public void OnOpen()
    {
        objets = new List<ObjetCombat>();
        for(int i = 0; i < listObjets.Count; ++i) {
            if (objetsCount[i] > 0) {
                listObjets[i].gameObject.SetActive(true);
                listObjets[i].OnSelect(false);
                listObjets[i].SetName(objetsCount[i]);
                objets.Add(listObjets[i]);
            } else {
                listObjets[i].gameObject.SetActive(false);
            }
        }

        gameObject.SetActive(true);
        ActionsManager._instance.enabled = false;
        if (objets.Count > 0) {
            currentObjet = 0;
            objets[currentObjet].OnSelect(true);
        }
    }

    public void OnClose()
    {
        InfoCombat._instance.gameObject.SetActive(false);
        if (objets.Count > 0) {
            objets[currentObjet].OnSelect(false);
        }
        ActionsManager._instance.enabled = true;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (objets.Count > 0) {
            int dir = InputController.downDown ? 1 : InputController.upDown ? -1 : 0;

            if (dir != 0) {
                SelectNextAction(dir);
            }

            if (InputController.ineraction) {
                objets[currentObjet].OnChooseAction();
            }
        }            

        if (InputController.exit) {
            MusicControler._instance.AudioClose();
            OnClose();
        }
    }

    public string AddObjet(Bug_Objet bug_Objet)
    {
        objetsCount[(int)bug_Objet]++;
        return listObjets[(int)bug_Objet].name;
    }

    public bool isAttack()
    {
        return objets[currentObjet].isAttack();
    }

    void SelectNextAction(int dir)
    {
        int newAction = mod(currentObjet + dir, objets.Count);

        objets[currentObjet].OnSelect(false);

        objets[newAction].OnSelect(true);
        currentObjet = newAction;
    }

    public static int mod(int x, int m)
    {
        return (x % m + m) % m;
    }

    public void OnGetTarget(TargetCombat targetCombat)
    {
        objetsCount[listObjets.IndexOf(objets[currentObjet])]--;
        OnClose();
        ActionsManager._instance.OnGetTarget(targetCombat, objets[currentObjet]);
    }
}
