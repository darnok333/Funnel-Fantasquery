using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public struct CombatRequest
{
    public TargetCombat actor;
    public TargetCombat target;
    public ActionCombat action;
}

public class CombatManager : MonoBehaviour
{
    public static CombatManager _instance;

    [SerializeField]
    GameObject prefabDamage, prefabHeal, prefabEC, EndScreen, DieScreen;

    [SerializeField]
    Transform EnemysRoot, RootUI;

    List<CombatRequest> combatRequests;
    
    bool winCombat = false;
    
    void Awake()
    {
        _instance = this;
        combatRequests = new List<CombatRequest>();
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if(combatRequests.Count > 0) {
            ExecuteNextRequest();
        }
    }

    public void OnReceiveRequest(CombatRequest combatRequest)
    {
        combatRequests.Add(combatRequest);
    }

    void ExecuteNextRequest()
    {        
        while(combatRequests.Count > 0 && combatRequests[0].actor.isDead()) {
            combatRequests.RemoveAt(0);
        }

        if (combatRequests.Count > 0) {
            CheckIfTargetStillAlive();
            combatRequests[0].actor.MoveFrontCharacter();
            this.enabled = false;
        }
    }

    void CheckIfTargetStillAlive()
    {
        if(combatRequests[0].target.isDead()) {
            CombatRequest combatRequest = combatRequests[0];
            combatRequest.target = TargetsManager._instance.GetNewAliveTarget(combatRequests[0].target);
            combatRequests[0] = combatRequest;
        }
    }

    public void InitCombat(List<TargetCombat> enemys)
    {
        RootUI.gameObject.SetActive(true);
        winCombat = false;
        combatRequests = new List<CombatRequest>();
        PlayersManager._instance.InitCombat();
        TargetsManager._instance.CleanEnemys();
        float startY = 2f;
        float endY = -1f;

        if (enemys.Count > 1) {
            float space = (endY - (startY)) / (enemys.Count - 1);
            for (int i = 0; i < enemys.Count; ++i) {
                TargetsManager._instance.AddEnemy(Instantiate(enemys[i], EnemysRoot.position + Vector3.up * (startY + space * i), Quaternion.identity, EnemysRoot));
            }
        } else {
            TargetsManager._instance.AddEnemy(Instantiate(enemys[0], EnemysRoot.position + Vector3.up * (startY + endY) / 2.0f, Quaternion.identity, EnemysRoot));
        }
    }

    public bool isWin()
    {
        return winCombat;
    }

    public void InstantiateParticle()
    {
        combatRequests[0].action.InstantiateParticle(combatRequests[0].actor, combatRequests[0].target);
    }

    public void ExecuteAction()
    {
        InfoController._instance.SetInfo(combatRequests[0].action.name);
        combatRequests[0].action.Execute(combatRequests[0].actor, combatRequests[0].target);
        combatRequests[0].actor.UseMana(combatRequests[0].action.manaCost);
    }

    public void MoveBackCharacter()
    {
        InfoController._instance.gameObject.SetActive(false);
        combatRequests[0].actor.MoveBackCharacter();
        combatRequests.RemoveAt(0);
    }

    public void InstantiateDamagePrefab(Vector3 pos, int damage)
    {
        if (damage != 0)
        Instantiate(prefabDamage, pos, Quaternion.identity).GetComponentInChildren<Text>().text = damage.ToString();
    }

    internal void InstantiateHealPrefab(Vector3 pos, int damage)
    {
        Instantiate(prefabHeal, pos, Quaternion.identity).GetComponentInChildren<Text>().text = damage.ToString();
    }

    internal void InstantiateManaPrefab(Vector3 pos, int damage)
    {
        Instantiate(prefabEC, pos, Quaternion.identity).GetComponentInChildren<Text>().text = damage.ToString();
    }

    public void EndCombat(bool win)
    {
        TargetsManager._instance.OnClose();
        ActionsManager._instance.OnClose();
        PlayersManager._instance.enabled = false;
        RootUI.gameObject.SetActive(false);
        InfoController._instance.gameObject.SetActive(false);

        if (win) {
            winCombat = true;
            PlayersManager._instance.OnWin();

            if (CombatTrigger.combatTrigger.boss) {
                EndScreen.SetActive(true);
                MusicControler._instance.StartCouroutineFade(0.5f, 0, MusicClip.WIN);
            } else {
                WinScreen._instance.OnWinScreen();
            }

            Destroy(CombatTrigger.combatTrigger.gameObject);
        } else {
            DieScreen.SetActive(true);
        }
    }

    public void BackToExplo()
    {
        gameObject.SetActive(false);
        PlayerController._instance.enabled = true;
    }

    public int GetAnimState()
    {
        return combatRequests[0].action.GetAnimState();
    }

    public void Restart()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
