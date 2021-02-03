using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Dictionary<string, Vector2> _positions = new Dictionary<string, Vector2>
    {
        { "q", new Vector2(-1, 1) }, { "w", new Vector2(0, 1) }, { "e", new Vector2(1, 1) }, 
        { "a", new Vector2(-1, 0) },                              { "d", new Vector2(1, 0) }, 
        { "z", new Vector2(-1, -1) }, { "x", new Vector2(0, -1) }, { "c", new Vector2(1, -1) }
    };

    AttackPoints _hitPoints;
    bool _canAct;

    public IEnumerator HandleTurn()
    {
        Debug.Log("player turn");

        while (true)
        {
            Debug.Log("player loop");

            if (Input.anyKeyDown && _positions.ContainsKey(Input.inputString))
                _hitPoints.ToggleAttackPoint(_positions[Input.inputString]);

            if (Input.GetButtonDown("Advance"))
                yield break;

            yield return null;
        }
    }

    public void WaitForTurn()
    {
    }

    void Start()
    {
        _hitPoints = GetComponentInChildren<AttackPoints>();
    }

    void Update()
    {
        // HandleKeyPresses();
    }

    private void HandleKeyPresses()
    {
        if (!_canAct || !Input.anyKeyDown)
            return;

        var key = Input.inputString;
        if (!_positions.ContainsKey(key))
            return;

        _hitPoints.ToggleAttackPoint(_positions[key]);
    }
}
