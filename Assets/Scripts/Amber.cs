using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Amber : MonoBehaviour
{
    public Vector2 size = Vector2.one;

    void Update()
    {
        var scale = transform.localScale;
        var t = Mathf.PerlinNoise(transform.position.x, transform.position.y) + Mathf.Sin(Time.time * 3);
        t = Mathf.Clamp01(t);
        scale.z = Mathf.Lerp(size.x, size.y, t);
        transform.localScale = scale;
    }
}
