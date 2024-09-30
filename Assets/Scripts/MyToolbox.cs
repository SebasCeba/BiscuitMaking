using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyToolbox
{
    public static Vector2 GetRandomVector2(float _minX, float _maxX, float _minY, float _maxY)
    {
        Vector2 value = new Vector2();
        value.x = Random.Range(_minX, _maxX);
        value.y = Random.Range(_minY, _maxY);
        return value;
    }
    public static Vector2 FallingRandomVector2(float _minX, float _maxX, float _maxY)
    {
        Vector2 value = new Vector2();
        value.x = Random.Range(_minX, _maxX);
        value.y = Random.Range(_maxY, _maxY);
        return value;
    }
}
