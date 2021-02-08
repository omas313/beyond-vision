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
    [SerializeField] ParticleSystem _loopingFireParticles;
    [SerializeField] GameObject _nextStepGameObject;

    List<GameObject> _path = new List<GameObject>();
    Transform _playerTransform;
    CameraShaker _cameraShaker;
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
        _loopingFireParticles.gameObject.SetActive(false);
        _nextStepGameObject.GetComponent<SpriteRenderer>().enabled = false;
    }

    public void Show()
    {
        _spriteRenderer.enabled = true;
        _loopingFireParticles.gameObject.SetActive(true);
        _nextStepGameObject.GetComponent<SpriteRenderer>().enabled = false;
    }

    public IEnumerator Die()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().simulated = false;
        
        _loopingFireParticles.Stop();
        _deathParticles.Play();
        yield return AudioManager.Instance.PlayPlayerDeathSound();
        yield return new WaitUntil(() => !_deathParticles.isPlaying);
        Died?.Invoke(this);
        Destroy(gameObject);
    }

    public void TryMove()
    {
        var gridOffset = Grid.WorldToGridPosition(_playerTransform.position - transform.position);
        if (OneSquareAway(gridOffset))
            return;

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
        _cameraShaker = FindObjectOfType<CameraShaker>();
        // DrawPath();
    }

    IEnumerator Attack()
    {
        _cameraShaker.ShakeCamera();
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

    bool OneSquareAway(Vector2 gridOffset) => 
        (Math.Abs(gridOffset.x) == 1 && gridOffset.y == 0)
        || (Math.Abs(gridOffset.y) == 1 && gridOffset.x == 0)
        || (Math.Abs(gridOffset.x) == 1 && Math.Abs(gridOffset.y) == 1);

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
