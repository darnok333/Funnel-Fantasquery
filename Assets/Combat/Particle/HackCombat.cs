using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;

public class HackCombat : MonoBehaviour
{
    [SerializeField]
    ScriptableRendererFeature scriptableRendererFeature;
    [SerializeField]
    Material _MatPostProcess;
    float value;
    int dir;

    private void Start()
    {
        dir = 1;
        value = 0;
        if(scriptableRendererFeature != null)
            scriptableRendererFeature.SetActive(true);
    }

    private void Update()
    {
        value += Time.deltaTime * dir;
        value = Mathf.Clamp01(value);
        _MatPostProcess.SetFloat("_Fade", value);

        if (value == 0 && dir == -1) {
            CombatManager._instance.MoveBackCharacter();
            enabled = false;
            if (scriptableRendererFeature != null)
                scriptableRendererFeature.SetActive(false);
            Destroy(gameObject);
        } else if (value == 1 && dir == 1) {
            CombatManager._instance.ExecuteAction();
            dir = -1;
        }
    }
}
