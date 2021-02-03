using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Font : MonoBehaviour
{
    void Start()
    {
        var tmpro = GetComponent<TextMeshPro>();
        tmpro.material.mainTexture.filterMode = FilterMode.Point;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
