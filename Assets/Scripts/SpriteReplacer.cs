#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SpriteReplacer : MonoBehaviour
{
    [SerializeField] Sprite original, replacement;
    
    [ContextMenu("Replace Sprites")]
    void Replace ()
    {
        foreach(var sr in FindObjectsOfType<SpriteRenderer>())
        {
            if(sr.sprite.Equals(original))
            {
                Undo.RecordObject(sr, "Changed Sprite.");
                sr.sprite = replacement;
            }
        }
    }
}
#endif