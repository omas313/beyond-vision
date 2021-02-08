using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class UIMainMenuController : MonoBehaviour
{
    [SerializeField] bool _shouldPlayIntro = true;
    [SerializeField] UIAnimatedImage _coverImage;

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
        _coverImage.Play();

        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => _coverImage.IsAnimationCompleted);
        yield return new WaitForSeconds(0.25f);

        GameManager.Instance.LoadLevelScene();
    }
}
