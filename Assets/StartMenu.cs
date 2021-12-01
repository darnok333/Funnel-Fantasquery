using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    [SerializeField]
    bool restart;
    [SerializeField]
    GameObject Player;

    void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        if (!restart)
            MusicControler._instance.StartCouroutineFade(0, 1, MusicClip.MENU);
    }

    // Update is called once per frame
    void Update()
    {
        if (InputController.ineraction) {
            if (restart) {
                CombatManager._instance.Restart();
            } else {
                gameObject.SetActive(false);
                Player.SetActive(true);
                MusicControler._instance.AudioSelect();
            }
        }
    }
}
