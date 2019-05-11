using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class HeatDistortion : MonoBehaviour
{

    public Material distortion;

    void OnRenderImage (RenderTexture source, RenderTexture target)
    {
        Graphics.Blit(source, target, distortion);
    }
}
