using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PlatformRimShadows : MonoBehaviour
{
    void Start()
    {
        if (Camera.main != null)
            Camera.main.depthTextureMode = DepthTextureMode.Depth;
    }

}
