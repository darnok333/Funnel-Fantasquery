using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CombatTrigger : MonoBehaviour
{
    public static CombatTrigger combatTrigger;

    [SerializeField]
    public bool boss;
    [SerializeField]
    List<TargetCombat> _Enemys;
    [SerializeField]
    List<Bug_Objet> _Objets;
    [SerializeField]
    ScriptableRendererFeature scriptableRendererFeature;
    [SerializeField]
    Material _HexFadeMat;
    bool start = false;
    float valueFading;

    int dir;

    private void Update()
    {
        if(start) {
            valueFading += Time.deltaTime * dir;
            _HexFadeMat.SetFloat("_Fade", valueFading);

            if (valueFading <= -1.5f && dir == -2) {
                start = false;
                scriptableRendererFeature.SetActive(false);
            } else if (valueFading >= 1.5f && dir == 1) {
                CombatManager._instance.gameObject.SetActive(true);
                PlayersManager._instance.InitCombat();
                CombatManager._instance.enabled = true;
                dir = -2;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        LaunchCombat();
    }

    void LaunchCombat()
    {
        combatTrigger = this;
        PlayerController._instance.enabled = false;
        dir = 1;
        valueFading = -1.5f;
        start = true;
        scriptableRendererFeature.SetActive(true);

        MusicControler._instance.StartCouroutineFade(0.25f, 0, boss ? MusicClip.BOSS : MusicClip.COMBAT, 0.8f);
        CombatManager._instance.InitCombat(_Enemys);
    }

    public int GetCE()
    {
        int ce = 0;
        foreach (EnemyCombat enemi in _Enemys) {
            ce += enemi.CE;
        }

        return ce;
    }

    public int GetXP()
    {
        int xp = 0;
        foreach (EnemyCombat enemi in _Enemys) {
            xp += enemi.XP;
        }

        return xp;
    }

    public List<string> GetObjets()
    {
        List<string> names = new List<string>();
        for (int i = 0; i < _Objets.Count; ++i) {
            names.Add(ObjetsManager._instance.AddObjet(_Objets[i]));
        }

        return names;
    }

    public List<string> GetComposants()
    {
        return new List<string>();
    }
}
