using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    [SerializeField] int _shakeCount = 5;
    [SerializeField] float _speed = 20f;

    Camera _camera;
    Vector3 _initialPosition;

    // [ContextMenu("shake")]
    public void ShakeCamera()
    {
        StartCoroutine(Shake());
    }

    void Awake()
    {
        _camera = Camera.main;
        _initialPosition = transform.position;
    }

    IEnumerator Shake()
    {
        Vector3 targetPosition;
        int shakeCount = (int)UnityEngine.Random.Range(_shakeCount, _shakeCount * 1.5f);

        for (int i = 0; i <= shakeCount; i++)
        {
            if (i == shakeCount)
                targetPosition = _initialPosition;
            else
            {
                targetPosition = UnityEngine.Random.insideUnitCircle * 0.5f + (Vector2)transform.position;
                targetPosition.z = _initialPosition.z;
            }
            
            while (Vector2.Distance(transform.position, targetPosition) > 0.1f)
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * _speed);
                yield return null;
            }
        }

        transform.position = _initialPosition;
    }
}
