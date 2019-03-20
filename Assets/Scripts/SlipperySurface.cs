using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlipperySurface : MonoBehaviour
{

    new public SpriteRenderer renderer;
    public PlayerColors playerColors;
    public float randColor = 0.3f;

    public int playerIndex;

    void Start()
    {
        Reset();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        for (int i = 0; i < playerColors.Players; i++)
        {
            if (collision.collider.gameObject.name.Equals("Bleep" + i))
            {
                playerIndex = i;

                renderer.color = RandomizeColor(playerColors.GetParticleColor(playerIndex), randColor);
                renderer.enabled = true;
                Destroy(collision.collider.gameObject);
            }
        }
    }

    public void Reset()
    {
        playerIndex = -1;
        if (renderer != null)
        {
            renderer.color = Color.black;
            renderer.enabled = false;
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
