using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChibiController : MonoBehaviour
{
    [SerializeField]
    Animator _Anim;

    float deltaX, deltaY;
    float lastX, lastY;

    // Start is called before the first frame update
    void Start()
    {
        lastX = transform.position.x;
        lastY = transform.position.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        SetDelta();
    }

    void SetDelta()
    {
        deltaX = transform.position.x - lastX;
        deltaY = transform.position.y - lastY;

        if (Mathf.Abs(deltaX) > 0 || Mathf.Abs(deltaY) >0) {
            _Anim.SetFloat("deltaX", Mathf.Abs(deltaX) > 0 ? Mathf.Sign(deltaX) : 0);
            _Anim.SetFloat("deltaY", Mathf.Abs(deltaY) > 0 ? Mathf.Sign(deltaY) : 0);
            _Anim.SetFloat("speed", 1);
        } else {
            _Anim.SetFloat("speed", 0);
        }

        lastX = transform.position.x;
        lastY = transform.position.y;
    }
}
