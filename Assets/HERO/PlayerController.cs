using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public static PlayerController _instance;

    [SerializeField]
    ChibiController chibi;
    [SerializeField]
    Rigidbody2D _RGBD;
    [SerializeField]
    float _Speed;
    float x, y;

    [SerializeField]
    GameObject _Menu;
    

    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        _instance = this;
    }

    private void OnEnable()
    {
        MusicControler._instance.StartCouroutineFade(1, 0, MusicClip.EXPLORATION);
        chibi.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        x = InputController.horizontal;
        y = InputController.vertical;
        Vector2 tmp = new Vector2(x, y).normalized;
        x = tmp.x;
        y = tmp.y;
    }

    private void FixedUpdate()
    {
        _RGBD.velocity = new Vector2(x, y).normalized * _Speed;
    }

    private void OnDisable()
    {
        _RGBD.velocity = Vector2.zero;
        chibi.enabled = false;
    }
}
