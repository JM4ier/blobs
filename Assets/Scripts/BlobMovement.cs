using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobMovement : MonoBehaviour
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
    public Transform particleSpawn;
    public float shootDelay = 0.05f;
    public PlayerColors colors;
    public int playerIndex;
    #endregion
    #region Variables
    private float hspeed;
    private bool grounded;
    private float lastShot;
    #endregion

    void Start()
    {
        horizontalSpeed = Mathf.Abs(horizontalSpeed);
    }

    void Update()
    {
        grounded = false;
        if (Physics2D.Raycast(blobBottom.position, -blobBottom.up, 0.5f, jumpable))
        {
            grounded = true;
        }

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

        if (grounded)
        {
            if (Input.GetKeyDown(movement.jump))
            {
                blob.AddForce(blob.transform.up * 7.5f, ForceMode2D.Impulse);
            }
        }
        else
        {
            // correctly rotate blob
            blob.angularVelocity = -5f * blob.rotation;

        }
    }

    void Shoot()
    {
        if (Time.time - lastShot > shootDelay)
        {
            lastShot = Time.time;
            var p = Instantiate(particle, particleSpawn.position + (Vector3)Random.insideUnitCircle * 0.2f, Quaternion.identity);
            p.name = "Bleep0";
            Destroy(p, 5f);
            var rig = p.GetComponent<Rigidbody2D>();
            rig.angularVelocity = 360f * Random.value;
            rig.velocity = 2f * blob.velocity;
            var renderer = p.GetComponent<SpriteRenderer>();
            renderer.color = RandomizeColor(colors.GetParticleColor(playerIndex), 0.2f);
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

    void FixedUpdate()
    {
        var localVelocity = blob.transform.InverseTransformVector(blob.velocity);
        localVelocity.x = hspeed;
        var targetVelocity = blob.transform.TransformVector(localVelocity);
        blob.AddForce(new Vector2(targetVelocity.x, targetVelocity.y) - blob.velocity);
    }
}
