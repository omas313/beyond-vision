using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameCanvasManager : MonoBehaviour
{
    public event Action ReloadRequested;
    public event Action LevelCompleted;

    [SerializeField] Text _enemyCountText;
    [SerializeField] Text _mpCountText;
    [SerializeField] Text _levelText;
    [SerializeField] UIAnimatedImage _uiUnconverImage;
    [SerializeField] UIAnimatedImage _uiCoverImage;
    [SerializeField] GameObject _reloadPanel;
    [SerializeField] Animator _levelCompleteAnimator;

    TurnController _turnController;
    PlayerController _playerController;

    public void Init(int level)
    {
        _uiCoverImage.Deactivate();
        _uiUnconverImage.Play();
        _reloadPanel.SetActive(false);    


        if (_turnController == null)
        {
            _turnController = FindObjectOfType<TurnController>();    
            _turnController.EnemyCountChanged += OnEnemyCountChanged;
            _turnController.LevelFailed += OnLevelFailed;
            _turnController.LevelCompleted += OnLevelCompleted;
        }

        if (_playerController == null)
        {
            _playerController = FindObjectOfType<PlayerController>();
            _playerController.MPChanged += OnPlayerMPChanged;
        }

        OnEnemyCountChanged(_turnController.EnemyCount);
        OnPlayerMPChanged(_playerController.MP);
        _levelText.text = level.ToString();
    }

    void OnLevelFailed()
    {
        // show the ui
        StartCoroutine(ShowReloadCanvas());
    }

    void OnLevelCompleted()
    {
        StartCoroutine(ShowLevelCompleted());
    }

    IEnumerator ShowReloadCanvas()
    {
        _uiCoverImage.Play();
        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => _uiCoverImage.IsAnimationCompleted);

        _reloadPanel.SetActive(true);    
        
        while (true)
        {
            if (Input.GetButtonDown("Advance") && !UIPaginatedTextController.IsPaginationActive)
            {
                ReloadRequested?.Invoke();
                yield break;
            }

            yield return null;
        }
    }

    IEnumerator ShowLevelCompleted()
    {
        _levelCompleteAnimator.SetTrigger("Enter");
        yield return new WaitForSeconds(0.1f); // takes some milliseconds to set the new animation
        yield return new WaitUntil(() => _levelCompleteAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);

        yield return new WaitForSeconds(1f); 

        _levelCompleteAnimator.SetTrigger("Exit");
        yield return new WaitForSeconds(0.1f); // takes some milliseconds to set the new animation
        yield return new WaitUntil(() => _levelCompleteAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);

        LevelCompleted?.Invoke();
    }

    void OnEnemyCountChanged(int count) => _enemyCountText.text = count.ToString();

    void OnPlayerMPChanged(int mp) => _mpCountText.text = mp.ToString();
}
