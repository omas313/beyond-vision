using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Grid 
{
    private const float GRID_SIZE = 2.25f;

    public static Vector2 WorldToGridPosition(Vector2 worldPosition)
    {
        return new Vector2(
            worldPosition.x / GRID_SIZE,
            worldPosition.y / GRID_SIZE);
    }

    public static Vector2 GridToWorldPosition(Vector2 gridPosition)
    {
        return new Vector2(
            gridPosition.x * GRID_SIZE,
            gridPosition.y * GRID_SIZE);
    }

    public static Vector2 WorldToGridPosition(float worldX, float worldY) => new Vector2(worldX / GRID_SIZE, worldY / GRID_SIZE);

    public static Vector2 GridToWorldPosition(int gridX, int gridY) => new Vector2(gridX * GRID_SIZE,gridY * GRID_SIZE);
}
