using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPoint : MonoBehaviour
{
    public bool HasEnemy => Enemy != null;
    public bool IsActive { get; private set; }
    public Vector2 Position => transform.position;
    public Enemy Enemy { get; private set; }
    

    AttackBlock _attackBlock;
    SpriteRenderer _spriteRenderer;
    Collider2D _collider;

    public void Toggle()
    {
        IsActive = !IsActive;
        _spriteRenderer.enabled = IsActive;
        _collider.enabled = IsActive;
    }

    public void Deactivate()
    {
        IsActive = false;
        _spriteRenderer.enabled = IsActive;
        _collider.enabled = IsActive;
    }

    public IEnumerator PerformAttack()
    {
        if (HasEnemy)
            Enemy.Show();

        yield return new WaitForSeconds(0.25f);
        _attackBlock.Animate();
        yield return new WaitUntil(() => !_attackBlock.IsAnimationPlaying);
        _attackBlock.Deactivate();

        if (HasEnemy)
            yield return Enemy.Die();
    }

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
        _spriteRenderer.enabled = false;
        _collider.enabled = false;
    }

    void Start()
    {
        _attackBlock = GetComponentInChildren<AttackBlock>();
        _attackBlock.Deactivate();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Enemy = other.GetComponent<Enemy>();
    }

    [ContextMenu("Set name to position")]
    void SetName()
    {
        name = $"{transform.position.x}, {transform.position.y}";
    }

    # region Debugging
    
    public void Toggle(float duration)
    {
        StartCoroutine(ToggleForSeconds(duration));
    }

    IEnumerator ToggleForSeconds(float duration)
    {
        _spriteRenderer.enabled = true;
        yield return new WaitForSeconds(duration);
        _spriteRenderer.enabled = false;
    }

    #endregion
}
