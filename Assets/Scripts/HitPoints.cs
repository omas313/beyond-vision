using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPoints : MonoBehaviour
{
    [SerializeField] float _toggleDuration = 0.2f;

    Dictionary<Vector2, HitPoint> _hitPoints = new Dictionary<Vector2, HitPoint>();

    void Awake()
    {
        foreach (var hitpoint in GetComponentsInChildren<HitPoint>())
            _hitPoints[hitpoint.transform.position] = hitpoint;
    }

    public void ToggleHitPoint(Vector2 position)
    {
        if (!_hitPoints.ContainsKey(position))
        {
            Debug.Log($"warning: trying to toggle hitpoint with key: {position}");
            return;
        }

        _hitPoints[position].Toggle(_toggleDuration);
    }
}
