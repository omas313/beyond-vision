using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPoints : MonoBehaviour
{
    // [SerializeField] float _toggleDuration = 0.2f;
    Dictionary<Vector2, AttackPoint> _attackPoints = new Dictionary<Vector2, AttackPoint>();

    public void ToggleAttackPoint(Vector2 position)
    {
        if (!_attackPoints.ContainsKey(position))
        {
            Debug.Log($"warning: trying to toggle hitpoint with key: {position}");
            return;
        }

        _attackPoints[position].Toggle();
    }

    public void DeactivateAll()
    {
        foreach (var attackPoint in _attackPoints.Values)
            if (attackPoint.IsActive)
                attackPoint.Deactivate();
    }

    public IEnumerator HandleTriggers()
    {
        Debug.Log("handling triggers");
        yield return new WaitForSeconds(0.1f); // for colliders to register

        foreach (var attackPoint in _attackPoints.Values)
            if (attackPoint.HasEnemy)
            {
                attackPoint.Deactivate();
                yield return attackPoint.PerformAttack();
            }
    }
    
    void Awake()
    {
        foreach (var hitpoint in GetComponentsInChildren<AttackPoint>())
            _attackPoints[hitpoint.transform.position] = hitpoint;
    }
}
