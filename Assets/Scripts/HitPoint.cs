using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPoint : MonoBehaviour
{
    SpriteRenderer _spriteRenderer;

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

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.enabled = false;
    }

}
