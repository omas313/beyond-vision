using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameCanvasManager : MonoBehaviour
{
    public event Action ReloadRequested;

    [SerializeField] Text _enemyCountText;
    [SerializeField] Text _mpCountText;
    [SerializeField] Text _levelText;
    [SerializeField] UIAnimatedFadeImage _fadeOutImage;
    [SerializeField] UIAnimatedFadeImage _fadeInImage;
    [SerializeField] GameObject _reloadPanel;

    TurnController _turnController;
    PlayerController _playerController;

    public void Init(int level)
    {
        _fadeInImage.Deactivate();
        _fadeOutImage.Play();
        _reloadPanel.SetActive(false);    

        _levelText.text = level.ToString();

        if (_turnController != null)
            _turnController.EnemyCountChanged -= OnEnemyCountChanged;

        _turnController = FindObjectOfType<TurnController>();    
        _turnController.EnemyCountChanged += OnEnemyCountChanged;
        _turnController.LevelFailed += OnLevelFailed;
        OnEnemyCountChanged(_turnController.EnemyCount);

        if (_playerController != null)
            _playerController.MPChanged -= OnPlayerMPChanged;

        _playerController = FindObjectOfType<PlayerController>();
        _playerController.MPChanged += OnPlayerMPChanged;
        OnPlayerMPChanged(_playerController.MP);
    }

    void OnLevelFailed()
    {
        // show the ui
        StartCoroutine(ShowReloadCanvas());
    }

    IEnumerator ShowReloadCanvas()
    {
        _fadeInImage.Play();
        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => _fadeInImage.IsAnimationCompleted);

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

    void OnEnemyCountChanged(int count)
    {
        _enemyCountText.text = count.ToString();
    }

    void OnPlayerMPChanged(int mp)
    {
        _mpCountText.text = mp.ToString();
    }
}
