using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public event Action Hit;
    public event Action<int> MPChanged;

    public int MP { get; private set; }

    [SerializeField] Sprite _blindfoldedSprite;
    [SerializeField] ParticleSystem _hitParticles;
    Dictionary<string, Vector2> _gridAttackPositions = new Dictionary<string, Vector2>
    {
        { "q", new Vector2(-1, 1) }, { "w", new Vector2(0, 1) }, { "e", new Vector2(1, 1) }, 
        { "a", new Vector2(-1, 0) },                              { "d", new Vector2(1, 0) }, 
        { "z", new Vector2(-1, -1) }, { "x", new Vector2(0, -1) }, { "c", new Vector2(1, -1) }
    };
    Sprite _normalSprite;
    AttackPoints _attackPoints;
    SpriteRenderer _spriteRenderer;

    public IEnumerator HandleTurn()
    {
        // Debug.Log("player turn");

        while (true)
        {
            // Debug.Log("player loop");

            if (ShouldToggleAttackPoint())
                ToggleAttackPoint();

            if (Input.GetButtonDown("Advance"))
                yield break;

            yield return null;
        }
    }

    public void TakeHit()
    {
        StartCoroutine(Death());
    }

    public void BlindfoldOff()
    {
        _spriteRenderer.sprite = _normalSprite;
    }

    public void BlindfoldOn()
    {
        _spriteRenderer.sprite = _blindfoldedSprite;
    }

    public void SetMP(int amount) => MP = amount;
    public void ReduceMP() => MP--;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();    
        _normalSprite = _spriteRenderer.sprite;
    }

    void Start()
    {
        _attackPoints = GetComponentInChildren<AttackPoints>();
    }
    
    bool ShouldToggleAttackPoint() => Input.anyKeyDown 
        && _gridAttackPositions.ContainsKey(Input.inputString) 
        && _attackPoints.IsAttackPointOnGrid(_gridAttackPositions[Input.inputString]);

    void ToggleAttackPoint()
    {
        var position = _gridAttackPositions[Input.inputString];

        if (_attackPoints.IsAttackPointActive(position))
        {
            MP++;
            MPChanged?.Invoke(MP);
            _attackPoints.ToggleAttackPoint(position);
        }
        else if (MP > 0)
        {
            MP--;
            MPChanged?.Invoke(MP);
            _attackPoints.ToggleAttackPoint(position);
        }
    }

    IEnumerator Death()
    {
        _hitParticles.Play();
        _spriteRenderer.enabled = false;
        yield return new WaitUntil(() => !_hitParticles.isPlaying);
        
        Hit?.Invoke();
    }
}
