using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionCombat : MonoBehaviour
{
    [SerializeField]
    protected Transform startParticle;
    protected Text _Name;
    [SerializeField]
    protected int state;
    [SerializeField]
    protected GameObject _Selection, _PrefabParticle;
    [SerializeField]
    protected int damage;
    [SerializeField]
    protected bool attack;
    public string name;
    public string info;
    public int manaCost;

    protected void Awake()
    {
        _Selection.SetActive(false);
        _Name = GetComponent<Text>();
        name = _Name.text;
    }

    public bool isAttack()
    {
        return attack;
    }

    public void OnSelect(bool b)
    {
        _Selection.SetActive(b);

        if (b) {
            InfoCombat._instance.SetInfo(info + (manaCost > 0 ? '(' + manaCost.ToString() + ')' : ""));
            MusicControler._instance.AudioSelect();
        }
    }

    public virtual void OnChooseAction()
    {
        TargetsManager._instance.enabled = true;
    }

    public virtual void Execute(TargetCombat actor, TargetCombat target)
    {
        if(attack)
            target.Hit((int)(damage * UnityEngine.Random.Range(0.75f, 1.25f)));
        else
            target.Heal((int)(damage * UnityEngine.Random.Range(0.75f, 1.25f)));
    }

    public virtual void InstantiateParticle(TargetCombat actor, TargetCombat target)
    {
        if (_PrefabParticle)
            Instantiate(_PrefabParticle, startParticle != null ? startParticle.position : target.transform.position, Quaternion.identity);
    }

    internal void SetColor(bool v)
    {
        if (v) {
            _Name.color = Color.white;
        } else {
            _Name.color = Color.gray;
        }
    }

    public int GetAnimState()
    {
        return state;
    }

    public virtual void SetName(bool rand, string randName = "Error")
    {
        if(rand) {
            _Name.color = Color.gray;
            _Name.text = randName;
        } else {
            _Name.color = Color.white;
            _Name.text = name;
        }
    }
}
