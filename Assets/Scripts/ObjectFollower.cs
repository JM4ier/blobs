using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFollower : MonoBehaviour
{



    public Transform target;
    [Range(0, 1)]
    public float moveFactor = 0.8f;

    void Update()
    {
        float lerpFac = 1 - Mathf.Pow(1 - moveFactor, Time.deltaTime);
        var t = Camera.main.transform;
        t.position = new Vector3(Mathf.Lerp(t.position.x, target.position.x, lerpFac),
        Mathf.Lerp(t.position.y, target.position.y, lerpFac), t.position.z);
    }
}
