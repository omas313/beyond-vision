using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingScript : MonoBehaviour
{
    void Update()
    {
        PrintViewportPosition();
    }

    void PrintViewportPosition()
    {
        Debug.Log(Camera.main.WorldToViewportPoint(transform.position));
    }
}
