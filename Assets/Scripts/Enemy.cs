using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public event Action<Enemy> Died;

    public int NumbersOfSquaresAway => CalculateNumbersOfSquaresAway();

    [SerializeField] int _stepsPerTurn = 1;
    [SerializeField] ParticleSystem _deathParticles;
    [SerializeField] GameObject _nextStepGameObject;

    List<GameObject> _path = new List<GameObject>();
    Transform _playerTransform;
    SpriteRenderer _spriteRenderer;

    public void ShowNextStep()
    {
        if (_playerTransform == null)
            _playerTransform = FindObjectOfType<PlayerController>().transform;

        _nextStepGameObject.transform.position = CalculateNextWorldStep();
        _nextStepGameObject.GetComponent<SpriteRenderer>().enabled = true;
    }

    public void Hide()
    {
        _spriteRenderer.enabled = false;
        _nextStepGameObject.GetComponent<SpriteRenderer>().enabled = false;
    }

    public void Show()
    {
        _spriteRenderer.enabled = true;
        _nextStepGameObject.GetComponent<SpriteRenderer>().enabled = false;
    }

    public IEnumerator Die()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().simulated = false;
        
        _deathParticles.Play();
        yield return AudioManager.Instance.PlayPlayerDeathSound();
        yield return new WaitUntil(() => !_deathParticles.isPlaying);
        Died?.Invoke(this);
        Destroy(gameObject);
    }

    public void Move()
    {
        var gridOffset = Grid.WorldToGridPosition(_playerTransform.position - transform.position);
        MoveNSquares(gridOffset);
    }

    public IEnumerator TryAttack()
    {
        var gridOffset = Grid.WorldToGridPosition(_playerTransform.position - transform.position);
        if (OneSquareAway(gridOffset))
        {
            Show();
            yield return AudioManager.Instance.PlayEnemyRevealSound();
            yield return Attack();
        }
        yield return null;
    }

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        _playerTransform = FindObjectOfType<PlayerController>().transform;        
        // DrawPath();
    }

    IEnumerator Attack()
    {
        yield return _playerTransform.GetComponent<PlayerController>().Die();
    }

    void MoveNSquares(Vector2 gridOffset)
    {
        var gridStep = new Vector2(Math.Sign(gridOffset.x), Math.Sign(gridOffset.y));
        var worldStep = Grid.GridToWorldPosition(gridStep);

        var nextPosition = (Vector2)transform.position + worldStep *_stepsPerTurn;
        if (IsValidPosition(nextPosition))
            transform.position = nextPosition;
        else
            transform.position = (Vector2)_playerTransform.position - worldStep;

    }

    bool IsValidPosition(Vector2 nextPosition) => nextPosition != (Vector2)_playerTransform.position;

    bool OneSquareAway(Vector2 offset) => 
        (Math.Abs(offset.x) == 1 && offset.y == 0)
        || (Math.Abs(offset.y) == 1 && offset.x == 0);

    Vector2 CalculateNextWorldStep()
    {
        var gridOffset = Grid.WorldToGridPosition(_playerTransform.position - transform.position);
        var gridStep = new Vector2(Math.Sign(gridOffset.x), Math.Sign(gridOffset.y));
        var worldStep = Grid.GridToWorldPosition(gridStep);
        var nextPosition = (Vector2)transform.position + worldStep * _stepsPerTurn;

        if (IsValidPosition(nextPosition))
            return nextPosition;
        else
            return (Vector2)_playerTransform.position - worldStep;
    }

    int CalculateNumbersOfSquaresAway()
    {
        var nextWorldStep = CalculateNextWorldStep();
        var gridOffset = Grid.WorldToGridPosition((Vector2)_playerTransform.position - nextWorldStep);

        if (gridOffset.x == 0)
            return (int)Math.Abs(gridOffset.y);
        else 
            return (int)Math.Abs(gridOffset.x);
    }

    void DrawPath()
    {
        // var offset = _playerTransform.position - transform.position;

        // var step = new Vector2(Math.Sign(offset.x), Math.Sign(offset.y));
        // var start = (Vector2)transform.position + step;
        // var end = (Vector2)_playerTransform.position - step;

        // foreach (var tile in _path)
        //     Destroy(tile);
        // _path.Clear();

        // var current = new Vector2(start.x, start.y);
        // var loops = 0;
        // while (current != end)
        // {
        //     _path.Add(Instantiate(_pathSquarePrefab, current, Quaternion.identity)); //, _pathParent));
        //     current += step;

        //     loops++;
        //     if (loops > 20)
        //     {
        //         Debug.Log($"more than 20 loops; breaking; current: {current}");
        //         break;
        //     }
        // } 
        // _path.Add(Instantiate(_pathSquarePrefab, end, Quaternion.identity)); //, _pathParent));

    }
}
