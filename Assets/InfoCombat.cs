using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoCombat : MonoBehaviour
{
    public static InfoCombat _instance;

    [SerializeField]
    Text text;

    void Awake()
    {
        _instance = this;
        gameObject.SetActive(false);
    }

    public void SetInfo(string s)
    {
        text.text = s;
        gameObject.SetActive(true);
    }
}
