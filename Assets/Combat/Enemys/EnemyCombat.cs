using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : TargetCombat
{
    [SerializeField]
    protected List<Bug_Action> _ActionsOnlyOnce;
    public float speed;
    float currentTime;
    public int CE, XP;

    public override void InitCombat()
    {
        base.InitCombat();
        currentTime = Random.Range(0, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime * speed;
        currentTime = Mathf.Min(1, currentTime);

        if (currentTime == 1) {
            OnReady();
        }
    }

    void OnReady()
    {
        enabled = false;
        ActionCombat action;
        if (_ActionsOnlyOnce.Count > 0 && Random.Range(0, 2) == 0) {
            int rand = Random.Range(0, _ActionsOnlyOnce.Count);
            Bug_Action tmp = _ActionsOnlyOnce[rand];
            _ActionsOnlyOnce.RemoveAt(rand);
            action = ActionsManager._instance.GetRandomAction(new List<Bug_Action>() { tmp });
        } else {
            action = ActionsManager._instance.GetRandomAction(_Actions);
        }

        CombatRequest combatRequest = new CombatRequest();
        combatRequest.actor = this;
        combatRequest.target = TargetsManager._instance.GetRandomTarget(action.isAttack());
        combatRequest.action = action;

        CombatManager._instance.OnReceiveRequest(combatRequest);
    }

    public override void OnEndAction()
    {
        base.OnEndAction();

        currentTime = 0;
        enabled = true;
    }
}
