using System;
using UnityEngine;

public class AttackBlock : MonoBehaviour
{
    public bool IsAnimationPlaying => _animation.isPlaying;
    
    SpriteRenderer _spriteRenderer;
    Animation _animation;

    public void Deactivate()
    {
        _spriteRenderer.enabled = false;   
        _animation.enabled = false; 
    }

    public void Animate()
    {
        _spriteRenderer.enabled = true;   
        _animation.enabled = true; 
        _animation.Play();
    }

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();    
        _animation = GetComponent<Animation>();    
    }
}
