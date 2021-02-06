using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class UIMainMenuController : MonoBehaviour
{
    [SerializeField] bool _shouldPlayIntro = true;
    [SerializeField] UIAnimatedFadeImage _fadeInImage;

    bool _hasRequestedStart;
    
    void Update()
    {
        if (ShouldStart())
        {
            _hasRequestedStart = true;
            StartCoroutine(LoadLevel());
        }
    }
    
    bool ShouldStart() => !UIPaginatedTextController.IsPaginationActive && !_hasRequestedStart && Input.GetButtonDown("Advance");

    IEnumerator LoadLevel()
    {
        _fadeInImage.Play();

        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => _fadeInImage.IsAnimationCompleted);
        yield return new WaitForSeconds(0.25f);

        GameManager.Instance.LoadLevelScene();
    }
}
