using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Dictionary<string, Vector2> _gridAttackPositions = new Dictionary<string, Vector2>
    {
        { "q", new Vector2(-1, 1) }, { "w", new Vector2(0, 1) }, { "e", new Vector2(1, 1) }, 
        { "a", new Vector2(-1, 0) },                              { "d", new Vector2(1, 0) }, 
        { "z", new Vector2(-1, -1) }, { "x", new Vector2(0, -1) }, { "c", new Vector2(1, -1) }
    };

    [SerializeField] Sprite _blindfoldedSprite;
    Sprite _normalSprite;
    AttackPoints _attackPoints;
    SpriteRenderer _spriteRenderer;
    bool _canAct;

    public IEnumerator HandleTurn()
    {
        Debug.Log("player turn");

        while (true)
        {
            Debug.Log("player loop");

            if (Input.anyKeyDown && _gridAttackPositions.ContainsKey(Input.inputString))
                _attackPoints.ToggleAttackPoint(_gridAttackPositions[Input.inputString]);

            if (Input.GetButtonDown("Advance"))
                yield break;

            yield return null;
        }
    }

    public void BlindfoldOff()
    {
        _spriteRenderer.sprite = _normalSprite;
    }

    public void BlindfoldOn()
    {
        _spriteRenderer.sprite = _blindfoldedSprite;
    }

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();    
        _normalSprite = _spriteRenderer.sprite;
    }

    void Start()
    {
        _attackPoints = GetComponentInChildren<AttackPoints>();
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
        if (!_gridAttackPositions.ContainsKey(key))
            return;

        _attackPoints.ToggleAttackPoint(_gridAttackPositions[key]);
    }

}
