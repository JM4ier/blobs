using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobMovement : MonoBehaviour, IReset
{
    #region Inspector Variables
    [System.Serializable]
    public struct MovementKeys
    {
        public KeyCode jump, left, right, shoot;
    }

    public Rigidbody2D blob;
    public Transform blobBottom;
    public MovementKeys movement;
    public float horizontalSpeed = 1f;
    public float horizontalAcceleration = 1f;
    public float jumpSpeed = 10f;
    public LayerMask jumpable;
    public BlobBehaviour graphics;
    public GameObject particle;
    public Transform particleSpawn, centerOfMass;
    public float shootDelay = 0.05f;
    public PlayerColors colors;
    public int playerIndex;
    public float groundedDistance = 0.2f;
    public GameObject deathEffect;
    public DarknessControl darkness;
    #endregion
    #region Variables
    private Vector3 startPosition;
    private float hspeed;
    private bool grounded;
    private float lastShot;
    public bool horizontalControlEnabled;
    #endregion

    void Start()
    {
        horizontalSpeed = Mathf.Abs(horizontalSpeed);
        blob.centerOfMass = blob.transform.InverseTransformPoint(centerOfMass.position);
        startPosition = blob.position;
        horizontalControlEnabled = true;
    }

    void Update()
    {

        var a = horizontalAcceleration * Time.deltaTime;
        bool inputGiven = false;
        if (Input.GetKey(movement.left))
        {
            hspeed -= a;
            inputGiven = true;
        }
        if (Input.GetKey(movement.right))
        {
            hspeed += a;
            inputGiven = true;
        }
        if (!inputGiven)
        {
            // revert to zero
            if (Mathf.Abs(hspeed) < a)
            {
                hspeed = 0;
            }
            else
            {
                hspeed += (hspeed < 0 ? 1 : -1) * a;
            }
        }
        hspeed = Mathf.Clamp(hspeed, -horizontalSpeed, horizontalSpeed);

        if (Input.GetKey(movement.shoot))
        {
            graphics.MouthOpened = true;
            Shoot();
        }
        else
        {
            graphics.MouthOpened = false;
        }

        grounded = false;
        int raycasts = 3;
        for (int i = -raycasts; i <= raycasts; i++)
        {
            var pos = blobBottom.position + blobBottom.right * (float)(i) / (float)(raycasts) * blobBottom.localScale.x;
            if (Physics2D.Raycast(pos, -blobBottom.up, groundedDistance, jumpable))
            {
                grounded = true;
                break;
            }
        }


        if (grounded)
        {
            if (Input.GetKeyDown(movement.jump))
            {
                // jump
                blob.AddForce(blob.transform.up * 7.5f, ForceMode2D.Impulse);
            }
            else
            {
                // pull downwards
                blob.AddForce(-blob.transform.up * 0.1f);
            }
        }
        else
        {
            // correctly rotate blob
            float max = 200f;
            blob.angularVelocity = Mathf.Clamp(-5f * blob.rotation, -max, max);
        }
    }

    void Shoot()
    {
        if (Time.time - lastShot > shootDelay)
        {
            lastShot = Time.time;
            var p = Instantiate(particle, particleSpawn.position + (Vector3)Random.insideUnitCircle * 0.2f, Quaternion.identity);
            p.name = "Bleep" + playerIndex;
            Destroy(p, 5f);
            var rig = p.GetComponent<Rigidbody2D>();
            rig.angularVelocity = 360f * Random.value;
            rig.velocity = 2f * blob.velocity;
            var renderer = p.GetComponent<SpriteRenderer>();
            renderer.color = RandomizeColor(colors.GetParticleColor(playerIndex), 0.2f);
        }
    }

    public void Reset()
    {
        FindObjectOfType<BlobScore>().IncreaseScore(playerIndex);
        var vfx = Instantiate(deathEffect, transform.position, Quaternion.identity);
        darkness.lightSources.Add(vfx.transform);
        Destroy(vfx, 3f);
        transform.position = startPosition;
        blob.velocity = Vector3.zero;
        blob.angularVelocity = 0f;
        hspeed = 0;
    }
    Color RandomizeColor(Color color, float diff)
    {
        return new Color(
            Mathf.Clamp01(color.r + (Random.value * 2 - 1) * diff),
            Mathf.Clamp01(color.g + (Random.value * 2 - 1) * diff),
            Mathf.Clamp01(color.b + (Random.value * 2 - 1) * diff)
        );
    }

    void FixedUpdate()
    {
        var localVelocity = blob.transform.InverseTransformVector(blob.velocity);
        if (horizontalControlEnabled)
        {
            localVelocity.x = hspeed;
        }
        var targetVelocity = blob.transform.TransformVector(localVelocity);
        blob.AddForce(new Vector2(targetVelocity.x, targetVelocity.y) - blob.velocity);
    }
}
