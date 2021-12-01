using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour
{
    public static WinScreen _instance;

    [SerializeField]
    Text _Info;
    
    List<string> objets;
    List<string> toDisplay;

    void Awake()
    {
        _instance = this;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (InputController.ineraction) {
            if (toDisplay.Count > 0) toDisplay.RemoveAt(0);
            if (toDisplay.Count == 0) {
                gameObject.SetActive(false);
                CombatManager._instance.BackToExplo();
            } else {
                _Info.text = toDisplay[0];
                MusicControler._instance.AudioSelect();
            }
        }
    }

    public void OnWinScreen()
    {
        MusicControler._instance.StartCouroutineFade(0.5f, 0, MusicClip.WIN);
        objets = CombatTrigger.combatTrigger.GetObjets();

        toDisplay = new List<string>();

        for(int i = 0; i < objets.Count; ++i) {
            toDisplay.Add("Got " + objets[i]);
        }

        if (toDisplay.Count > 0) {
            _Info.text = toDisplay[0];
            MusicControler._instance.AudioSelect();
        } else
            _Info.text = "";
        gameObject.SetActive(true);
    }
}
