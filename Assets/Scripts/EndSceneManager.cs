using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndSceneManager : MonoBehaviour
{
    [SerializeField] UIAnimatedImage _coverImage;
    bool _isLoading;

    void Start()
    {
        AudioManager.Instance.PlayEndSceneMusic();
    }

    void Update()
    {
        if (!_isLoading && Input.GetButtonDown("Advance"))
        {
            _isLoading = true;
            StartCoroutine(LoadMainMenu());
        }
    }

    IEnumerator LoadMainMenu()
    {
        _coverImage.Play();

        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => _coverImage.IsAnimationCompleted);
        yield return new WaitForSeconds(0.25f);

        GameManager.Instance.LoadMainMenu();
    }
}
