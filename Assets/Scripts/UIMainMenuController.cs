using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class UIMainMenuController : MonoBehaviour
{
    [SerializeField] bool _shouldPlayIntro = true;
    [SerializeField] GameObject _fadeOutImage;
    [SerializeField] GameObject _fadeInImage;

    bool _hasRequestedStart;
    
    void Awake()
    {
        _fadeOutImage.SetActive(true);
    }

    void Update()
    {
        if (ShouldStart())
        {
            _hasRequestedStart = true;
            StartCoroutine(LoadLevel());
        }
    }
    
    bool ShouldStart() => !PaginatedTextController.IsPaginationActive && !_hasRequestedStart && Input.GetButtonDown("Advance");

    IEnumerator LoadLevel()
    {
        _fadeInImage.SetActive(true);

        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => !_fadeInImage.GetComponent<Animation>().isPlaying);
        yield return new WaitForSeconds(0.25f);

        GameManager.Instance.LoadLevelScene();
    }
}
