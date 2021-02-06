using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public event Action<Enemy> Died;

    [SerializeField] ParticleSystem _deathParticles;
    [SerializeField] GameObject _nextStepGameObject;

    List<GameObject> _path = new List<GameObject>();
    Transform _playerTransform;
    SpriteRenderer _spriteRenderer;

    public void ShowNextStep()
    {
        if (_playerTransform == null)
            _playerTransform = FindObjectOfType<PlayerController>().transform;

        var worldOffset = _playerTransform.position - transform.position;
        var step = Grid.GridToWorldPosition(Math.Sign(worldOffset.x), Math.Sign(worldOffset.y));
        var nextPosition = (Vector2)transform.position + step;
        
        _nextStepGameObject.transform.position = nextPosition;
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
    }

    public IEnumerator Die()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().simulated = false;
        
        _deathParticles.Play();
        yield return new WaitUntil(() => !_deathParticles.isPlaying);
        Died?.Invoke(this);
        Destroy(gameObject);
    }

    public void PerformAction()
    {
        var offset = Grid.WorldToGridPosition(_playerTransform.position - transform.position);

        if (OneSquareAway(offset))
        {
            Show();
            Attack();
        }
        else
            MoveNSquares(offset, n: 1);
    }

    void Attack()
    {
        _playerTransform.GetComponent<PlayerController>().TakeHit();
        Show();
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

    void MoveNSquares(Vector2 offset, int n)
    {
        // todo: implement movement by n, must check if n > dist to player then just move next to player

        if (offset.x != 0)
            offset.x = Math.Sign(offset.x);
        if (offset.y != 0)
            offset.y = Math.Sign(offset.y);

        transform.Translate(Grid.GridToWorldPosition(offset));
    }

    bool OneSquareAway(Vector2 offset) => Math.Abs(offset.x) == 1 || Math.Abs(offset.y) == 1;

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
