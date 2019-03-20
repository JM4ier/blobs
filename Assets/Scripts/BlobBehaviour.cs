using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BlobBehaviour : MonoBehaviour
{
    public Rigidbody2D rig;

    [System.Serializable]
    public struct BlobSprites
    {
        public SpriteRenderer body, eyes, mouthClosed, mouthOpened;
    }

    public BlobSprites sprites;
    public Vector2 maxDisplacement = Vector2.one;
    private Vector2 displacement;
    public float displacementSpeed;

    public PlayerColors playerColors;
    public int playerIndex;

    public bool MouthOpened
    {
        get
        {
            return sprites.mouthOpened.enabled;
        }
        set
        {
            sprites.mouthOpened.enabled = value;
            sprites.mouthClosed.enabled = !value;
        }
    }
    void Start()
    {
        displacement = Vector2.zero;
        maxDisplacement.x = Mathf.Abs(maxDisplacement.x);
        maxDisplacement.y = Mathf.Abs(maxDisplacement.y);
    }

    void Update()
    {
        // updating color
        var col = playerColors.GetPlayerColor(playerIndex);
        col.a = 1;
        sprites.body.color = col;
        sprites.mouthClosed.color = col;
        sprites.mouthOpened.color = col;

        var threshold = 0.75f;
        var v = rig.velocity;
        v = rig.transform.InverseTransformVector(v);

        var d = displacement * Time.deltaTime;

        if (v.y < -threshold)
        {
            sprites.eyes.flipY = false;
            displacement.y -= d.y;
        }
        else if (v.y > threshold)
        {
            sprites.eyes.flipY = true;
            displacement.y += d.y;
        }
        if (v.x < -threshold)
        {
            sprites.mouthOpened.flipX = true;
            sprites.eyes.flipX = true;
            displacement.x -= d.x;
        }
        else if (v.x > threshold)
        {
            sprites.mouthOpened.flipX = false;
            sprites.eyes.flipX = false;
            displacement.x += d.x;
        }

        displacement.x = Mathf.Clamp(displacement.x, -maxDisplacement.x, maxDisplacement.x);
        displacement.y = Mathf.Clamp(displacement.y, -maxDisplacement.y, maxDisplacement.y);

        sprites.eyes.transform.localPosition = displacement;
    }
}
