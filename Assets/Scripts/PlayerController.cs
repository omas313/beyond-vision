using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Dictionary<string, Vector2> _positions = new Dictionary<string, Vector2>
    {
        { "q", new Vector2(-1, 1) }, { "w", new Vector2(0, 1) }, { "e", new Vector2(1, 1) }, 
        { "a", new Vector2(-1, 0) }, { "s", Vector2.zero }, { "d", new Vector2(1, 0) }, 
        { "z", new Vector2(-1, -1) }, { "x", new Vector2(0, -1) }, { "c", new Vector2(1, -1) }
    };

    HitPoints _hitPoints;

    void Start()
    {
        _hitPoints = GetComponentInChildren<HitPoints>();
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            var key = Input.inputString;
            if (!_positions.ContainsKey(key))
                return;

           _hitPoints.ToggleHitPoint(_positions[key]);
        }
    }
}
