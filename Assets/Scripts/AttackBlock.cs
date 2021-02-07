using System;
using UnityEngine;

public class AttackBlock : MonoBehaviour
{
    public bool IsAnimationPlaying => _animation.isPlaying;
    
    SpriteRenderer _spriteRenderer;
    CameraShaker _cameraShaker;
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

    // AnimationEventHandler
    public void OnAnimationImpact()
    {
        _cameraShaker.ShakeCamera();
    }

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();    
        _animation = GetComponent<Animation>();    
    }

    void Start()
    {
        _cameraShaker = FindObjectOfType<CameraShaker>();
    }
}
