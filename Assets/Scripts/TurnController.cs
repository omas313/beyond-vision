using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TurnController : MonoBehaviour
{
    public int Turn { get; private set; }


    [SerializeField] Text _text;

    PlayerController _playerController;
    List<Enemy> _enemies = new List<Enemy>();
    AttackPoints _attackPoints;

    bool IsFirstTurn => Turn == 0;
    bool _isPlayerTurn;
    bool _isHandlingTurn;

    void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();
        _attackPoints = FindObjectOfType<AttackPoints>();

        foreach (var enemy in FindObjectsOfType<Enemy>())
        {
            _enemies.Add(enemy);
            enemy.Died += OnEnemyDied;
        }

        Turn = 0;
        _isPlayerTurn = true;
        _playerController.BlindfoldOff();
        ShowEnemyTrails();
    }

    void ShowEnemyTrails()
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
        _text.text = _isPlayerTurn ? "player" : "enemy";

        if (IsFirstTurn || !_isHandlingTurn)
            StartCoroutine(AdvanceTurn());
    }

    // bool ShouldAdvanceTurn() => Input.GetButtonDown("Advance") && !_isHandlingTurn;

    IEnumerator AdvanceTurn()
    {
        Debug.Log("advance turn");

        _isHandlingTurn = true;
        
        if (Turn != 0)
        {
            HideEnemies();
            _playerController.BlindfoldOn();
            _isPlayerTurn = !_isPlayerTurn;
        }

        Turn++;

        if (_isPlayerTurn)
            yield return HandlePlayerTurn();
        else
            yield return HandleEnemyTurn();

        Debug.Log("finished turn");

        _isHandlingTurn = false;
    }

    IEnumerator HandlePlayerTurn()
    {
        yield return _playerController.HandleTurn();
    }

    IEnumerator HandleEnemyTurn()
    {
        Debug.Log("enemy turn");

        HandleEnemyAction();
        Debug.Log("handling attack points");
        yield return HandleAttackPoints();

        Debug.Log("enemy turn over, advancing");
    }

    void HandleEnemyAction()
    {
        foreach (var enemy in _enemies)
            enemy.PerformAction();
    }

    IEnumerator HandleAttackPoints()
    {
        yield return _attackPoints.HandleTriggers();
        _attackPoints.DeactivateAll();
    }
    
    void OnEnemyDied(Enemy enemy)
    {
        if (_enemies.Contains(enemy))
            _enemies.Remove(enemy);
        enemy.Died -= OnEnemyDied;
    }
}
