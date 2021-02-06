using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameCanvasManager : MonoBehaviour
{
    [SerializeField] Text _enemyCountText;
    [SerializeField] Text _mpCountText;
    [SerializeField] GameObject _fadeOutImage;

    TurnController _turnController;
    PlayerController _playerController;

    public void Init()
    {
        _fadeOutImage.SetActive(true);

        if (_turnController != null)
            _turnController.EnemyCountChanged -= OnEnemyCountChanged;

        _turnController = FindObjectOfType<TurnController>();    
        _turnController.EnemyCountChanged += OnEnemyCountChanged;
        OnEnemyCountChanged(_turnController.EnemyCount);

        if (_playerController != null)
            _playerController.MPChanged -= OnPlayerMPChanged;

        _playerController = FindObjectOfType<PlayerController>();
        _playerController.MPChanged += OnPlayerMPChanged;
        OnPlayerMPChanged(_playerController.MP);
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
