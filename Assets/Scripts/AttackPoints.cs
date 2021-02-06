using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttackPoints : MonoBehaviour
{
    public int ToggledAttackPointsCount => _attackPoints.Values.Where(ap => ap.IsActive).Count();

    // [SerializeField] float _toggleDuration = 0.2f;
    Dictionary<Vector2, AttackPoint> _attackPoints = new Dictionary<Vector2, AttackPoint>();

    public bool IsAttackPointActive(Vector2 gridPosition) => _attackPoints[gridPosition].IsActive;

    public bool IsAttackPointOnGrid(Vector2 gridPosition)
    {
        var viewportPosition = Camera.main.WorldToViewportPoint(_attackPoints[gridPosition].transform.position);
        return Grid.IsViewportPositionOnGrid(viewportPosition);
    }

    public void ToggleAttackPoint(Vector2 gridPosition)
    {
        if (!_attackPoints.ContainsKey(gridPosition))
        {
            Debug.Log($"warning: trying to toggle non-existent hitpoint with key: {gridPosition}, dictionary keys count: {_attackPoints.Count}");
            return;
        }

        _attackPoints[gridPosition].Toggle();
    }

    public void DeactivateAll()
    {
        foreach (var attackPoint in _attackPoints.Values)
            if (attackPoint.IsActive)
                attackPoint.Deactivate();
    }

    public IEnumerator HandleTriggers()
    {
        // Debug.Log("handling triggers");
        yield return new WaitForSeconds(0.1f); // for colliders to register

        foreach (var attackPoint in _attackPoints.Values)
        {
            if (!attackPoint.IsActive)
                continue;
                
            attackPoint.Deactivate();
            yield return attackPoint.PerformAttack();
        }
    }
    
    void Start()
    {
        foreach (var attackPoint in GetComponentsInChildren<AttackPoint>())
        {
            Vector2 gridPosition = Grid.WorldToGridPosition(attackPoint.transform.localPosition);
            _attackPoints[gridPosition] = attackPoint;
        }

        DeactivateAll();
    }
}
