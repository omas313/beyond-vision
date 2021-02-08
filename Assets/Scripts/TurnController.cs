using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TurnController : MonoBehaviour
{
    public event Action LevelCompleted;
    public event Action LevelFailed;
    public event Action<int> EnemyCountChanged;

    public int CurrentTurn { get; private set; }
    public int EnemyCount => _enemies.Count;

    PlayerController _playerController;
    List<Enemy> _enemies = new List<Enemy>();
    AttackPoints _attackPoints;

    bool IsInitTurn => CurrentTurn == 0;
    bool _isPlayerTurn;
    bool _isHandlingTurn;

    bool _isInitialized;
    bool _isLevelOver;

    public void Init()
    {
        _playerController = FindObjectOfType<PlayerController>();
        _playerController.Hit += OnPlayerHit;
        _attackPoints = FindObjectOfType<AttackPoints>();

        _enemies.Clear();
        foreach (var enemy in FindObjectsOfType<Enemy>())
        {
            _enemies.Add(enemy);
            enemy.Died += OnEnemyDied;
        }
        EnemyCountChanged?.Invoke(EnemyCount);

        _playerController.SetMP(EnemyCount + EnemyCount / 2);

        CurrentTurn = 0;
        _isPlayerTurn = true;
        _playerController.BlindfoldOff();
        ShowEnemyNextSteps();
        UpdateDangerAlert();

        _isLevelOver = false;
        _isInitialized = true;
    }

    void ShowEnemyNextSteps()
    {
        foreach (var enemy in _enemies)
            enemy.ShowNextStep();
    }
    
    void HideEnemies()
    {
        foreach (var enemy in _enemies)
            enemy.Hide();
    }

    void Update()
    {
        if (!_isInitialized || _isLevelOver)
            return;

        if (IsInitTurn || !_isHandlingTurn)
            StartCoroutine(AdvanceTurn());
    }

    IEnumerator AdvanceTurn()
    {
        // Debug.Log("advance turn");

        _isHandlingTurn = true;
        
        if (CurrentTurn != 0)
        {
            _playerController.BlindfoldOn();
            _isPlayerTurn = !_isPlayerTurn;
        }
        
        if (CurrentTurn == 1)
        {
        //     HideEnemies();
        }

        CurrentTurn++;
        // Debug.Log($"current turn {CurrentTurn}");

        if (_isPlayerTurn)
            yield return HandlePlayerTurn();
        else
        {
            yield return HandleEnemyTurn();
            UpdateDangerAlert();
        }

        // Debug.Log("finished turn");
        _isHandlingTurn = false;
    }

    IEnumerator HandlePlayerTurn()
    {
        yield return _playerController.HandleTurn();
    }

    IEnumerator HandleEnemyTurn()
    {
        // Debug.Log("enemy turn");

        HandleEnemyMovement();
        // Debug.Log("handling attack points");
        yield return HandleAttackPoints();
        yield return HandleEnemyAttack();

        // Debug.Log("enemy turn over, advancing");
    }

    void HandleEnemyMovement()
    {
        foreach (var enemy in _enemies)
            enemy.Move();
    }
    IEnumerator HandleEnemyAttack()
    {
        var enemies = new Enemy[_enemies.Count];
        _enemies.CopyTo(enemies);

        foreach (var enemy in enemies)
        {
            yield return enemy.TryAttack();
            
            if (_playerController.IsDead)
                yield break;
        }
    }

    IEnumerator HandleAttackPoints()
    {
        yield return _attackPoints.HandleTriggers();
        _attackPoints.DeactivateAll();
    }

    void UpdateDangerAlert()
    {
        int squaresAway = int.MaxValue;
        foreach (var enemy in _enemies)
            squaresAway = Math.Min(squaresAway, enemy.NumbersOfSquaresAway);

        if (squaresAway == 1)
            AudioManager.Instance.PlayHighBlipSound();
        else
            AudioManager.Instance.PlayLowBlipSound();
    }
    
    void DeregisterEvents()
    {
        _playerController.Hit -= OnPlayerHit;

        foreach (var enemy in _enemies)
            enemy.Died -= OnEnemyDied;
    }

    void OnPlayerHit()
    {
        StopAllCoroutines();
        _isLevelOver = true;
        DeregisterEvents();
        LevelFailed?.Invoke();
    }

    void OnEnemyDied(Enemy enemy)
    {
        if (_enemies.Contains(enemy))
            _enemies.Remove(enemy);
        enemy.Died -= OnEnemyDied;
        EnemyCountChanged?.Invoke(EnemyCount);

        if (EnemyCount == 0)
        {
            DeregisterEvents();
            _isLevelOver = true;
            LevelCompleted?.Invoke();
        }
    }
}
