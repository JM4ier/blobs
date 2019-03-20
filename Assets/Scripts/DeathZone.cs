using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DeathZone : MonoBehaviour
{

    void OnTriggerEnter2D (Collider2D collider)
    {
        var resetObjects = collider.gameObject.GetComponentsInChildren<IReset>().ToList();
        resetObjects.AddRange(collider.gameObject.GetComponentsInParent<IReset>());
        foreach(var obj in resetObjects)
        {
            obj.Reset();
        }
    }

}
