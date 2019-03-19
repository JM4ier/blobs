using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobMovement : MonoBehaviour
{
    [System.Serializable]
    public struct MovementKeys
    {
        public KeyCode jump, left, right;
    }

    public Rigidbody2D blob;
    public Transform blobBottom;
    public MovementKeys movement;
    public Vector2 speed = Vector2.one;
    private bool grounded = false;

    public LayerMask jumpable;

    void Update()
    {
        grounded = false;
        if (Physics2D.Raycast(blobBottom.position, -blobBottom.up, 0.1f, jumpable))
        {
            grounded = true;
        }

        var force = Vector2.zero;
        if (Input.GetKey(movement.jump) && grounded)
            force.y += speed.y;
        if (Input.GetKey(movement.right))
            force.x += speed.x;
        if (Input.GetKey(movement.left))
            force.x -= speed.x;

        force = blob.transform.TransformVector(force);
        blob.AddForce(force);
    }
}
