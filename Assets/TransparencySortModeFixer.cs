using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparencySortModeFixer : MonoBehaviour
{
    private void Start()
    {
        Camera cam = GetComponent<Camera>();
        cam.transparencySortAxis = Vector3.up;
        cam.transparencySortMode = TransparencySortMode.CustomAxis;
    }
}