using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlipperySurface : MonoBehaviour
{

    public BoxCollider2D[] colliders;
    new public SpriteRenderer renderer;
    public PlayerColors playerColors;
    public float randColor = 0.2f;

    void Start()
    {
        Reset();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        for (int i = 0; i < playerColors.Players; i++)
        {
            colliders[i].enabled = false;
            if (collider.name.Equals("Bleep" + i))
            {
                colliders[i].enabled = true;
                renderer.color = RandomizeColor(playerColors.GetParticleColor(i), randColor);
                renderer.enabled = true;
                Destroy(collider.gameObject);
            }
        }
    }

    public void Reset()
    {
        if (renderer != null)
        {
            renderer.enabled = false;
            for (int i = 0; i < playerColors.Players; i++)
            {
                colliders[i].enabled = false;
            }
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
