using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorExtensions
{
    public static Vector2 ToVector2(this Vector3 v)
    {
        return new Vector2(v.x, v.y);
    }

    public static Vector2[] ToVector2(this Vector3[] v)
    {
        Vector2[] output = new Vector2[v.Length];

        for (int i = 0; i < v.Length; ++i)
            output[i] = new Vector2(v[i].x, v[i].y);

        return output;
    }

    public static Vector3 ToVector3(this Vector2 v)
    {
        return new Vector3(v.x, v.y, 0.0f);
    }

    public static Vector3[] ToVector3(this Vector2[] v)
    {
        Vector3[] output = new Vector3[v.Length];

        for (int i = 0; i < v.Length; ++i)
            output[i] = new Vector3(v[i].x, v[i].y, 0.0f);

        return output;
    }
}

