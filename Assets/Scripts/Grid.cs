using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Grid 
{
    private const float GRID_SIZE = 2.25f;
    private const float VIEWPORT_MIN_X_BOUND = 0f;
    private const float VIEWPORT_MAX_X_BOUND = 0.8f;
    private const float VIEWPORT_MIN_Y_BOUND = 0f;
    private const float VIEWPORT_MAX_Y_BOUND = 1f;

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

    public static bool IsViewportPositionOnGrid(Vector3 viewportPosition) => 
        viewportPosition.x > VIEWPORT_MIN_X_BOUND && viewportPosition.x < VIEWPORT_MAX_X_BOUND
        && viewportPosition.y > VIEWPORT_MIN_Y_BOUND && viewportPosition.y < VIEWPORT_MAX_Y_BOUND;
}
