using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ATB_Barre : MonoBehaviour
{
    [SerializeField]
    PlayerCombat playerCombat;
    [SerializeField]
    RectTransform _Barre;
    public float speed;
    float initSpeed;
    float currentTime;

    private void Awake()
    {
        initSpeed = speed;
    }

    public void ResetSpeed()
    {
        speed = initSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime * speed;
        currentTime = Mathf.Min(1, currentTime);

        _Barre.localScale = new Vector3(currentTime, 1, 1);

        if(currentTime == 1) {
            OnReady();
        }
    }

    public void InitCombat()
    {
        currentTime = UnityEngine.Random.Range(0, 0.5f);
        _Barre.localScale = new Vector3(currentTime, 1, 1);
        enabled = true;
    }

    void OnReady()
    {
        enabled = false;
        PlayersManager._instance.PlayerReady(playerCombat);
    }

    public void Restart()
    {
        currentTime = 0;
        _Barre.localScale = new Vector3(currentTime, 1, 1);
        enabled = true;
    }
}
