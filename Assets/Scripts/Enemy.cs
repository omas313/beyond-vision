using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    Transform _playerTransform;

    void Start()
    {
        _playerTransform = FindObjectOfType<PlayerController>().transform;        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Move();        
    }

    void Move()
    {
        var offset = _playerTransform.position - transform.position;

        if (OneSquareAway(offset))
            return;
        
        MoveNSquares(offset, n: 1);
    }

    private void MoveNSquares(Vector2 offset, int n)
    {
        // todo: implement movement by n, must check if n > dist to player then just move next to player

        if (offset.x != 0)
            offset.x = Math.Sign(offset.x);
        if (offset.y != 0)
            offset.y = Math.Sign(offset.y);

        transform.Translate(offset);
    }

    private bool OneSquareAway(Vector2 offset) => Math.Abs(offset.x) == 1 || Math.Abs(offset.y) == 1;
}
