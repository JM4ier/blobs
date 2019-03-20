using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobFriction : MonoBehaviour
{
    public PhysicsMaterial2D material;
    public int index;

    public Transform bottom;

    public LayerMask surface;

    void Update()
    {
        int raycasts = 5;
        int frictionSum = 0, frictionObjects = 0;
        for (int i = -raycasts; i <= raycasts; i++)
        {
            var pos = bottom.position + Vector3.right * (float)(i) / (float)(raycasts) * bottom.localScale.x;
            var hit = Physics2D.Raycast(pos, -bottom.up, 0.1f, surface);
            if (!hit) continue;
            var surf = hit.collider.GetComponent<SlipperySurface>();
            if (surf == null) continue;
            frictionObjects++;
            if (surf.playerIndex == index || surf.playerIndex < 0) frictionSum++;
        }
        if (frictionObjects > 0)
        {
            material.friction = frictionSum / frictionObjects;
        }
    }
}
