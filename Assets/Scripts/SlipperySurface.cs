using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlipperySurface : MonoBehaviour
{

    public BoxCollider2D collider0, collider1;
    new public SpriteRenderer renderer;
    public Color color0, color1;

    public float randColor = 0.2f;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.name.Equals("Bleep0"))
        {
            collider0.enabled = false;
            collider1.enabled = true;
            renderer.color = RandomizeColor(color0, randColor);
            renderer.enabled = true;
            Destroy(collider.gameObject);
        }
        else if (collider.name.Equals("Bleep1"))
        {
            collider0.enabled = true;
            collider1.enabled = false;
            renderer.color = RandomizeColor(color1, randColor);
            renderer.enabled = true;
            Destroy(collider.gameObject);
        }
    }

    public void Reset()
    {
        if (renderer != null)
        {
            renderer.enabled = false;
            collider0.enabled = false;
            collider1.enabled = false;
        }
    }

    Color RandomizeColor(Color color, float diff)
    {
        return new Color(
            Mathf.Clamp01(color.r + (Random.value * 2 - 1) * diff),
            Mathf.Clamp01(color.g + (Random.value * 2 - 1) * diff),
            Mathf.Clamp01(color.b + (Random.value * 2 - 1) * diff)
        );
    }

}
