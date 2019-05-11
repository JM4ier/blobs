using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[ExecuteInEditMode]
public class DarknessControl : MonoBehaviour
{
    public List<Transform> lightSources;
    [SerializeField] private Material mat;
    Vector4[] src;

    void Awake()
    {
        src = new Vector4[100];
    }

    void Update()
    {
        lightSources.RemoveAll(t => t == null || !t);
        int length = Mathf.Min(lightSources.Count, 100);
        for (int i = 0; i < length; i++)
        {
            src[i] = new Vector3(lightSources[i].position.x, lightSources[i].position.y, lightSources[i].localScale.z);
        }
        mat.SetVectorArray("_Lights", src);
        mat.SetInt("_LightCount", length);
    }

}
