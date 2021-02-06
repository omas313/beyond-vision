using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAnimatedFadeImage : MonoBehaviour
{
    public bool IsAnimationCompleted { get; private set; }

    [SerializeField] bool _playOnAwake;
    [SerializeField] bool _disableOnAnimationCompleted;

    Animation _animation;
    Image _image;

    public void Play()
    {
        StartCoroutine(PlayAndDeactivate());
    }

    void Awake()
    {
        _animation = GetComponent<Animation>();    
        _image = GetComponent<Image>();

        if (_playOnAwake)
            Play();    
    }

    IEnumerator PlayAndDeactivate()
    {
        IsAnimationCompleted = false;
        GetComponent<Image>().enabled = true;
        _animation.Play();

        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => !_animation.isPlaying);

        if (_disableOnAnimationCompleted)
            GetComponent<Image>().enabled = false;
        IsAnimationCompleted = true;
    }
}
