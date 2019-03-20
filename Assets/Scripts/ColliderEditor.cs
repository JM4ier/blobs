using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ColliderEditor : MonoBehaviour
{

    public float partitionSize = 0.05f, width = 0.02f;
    public LayerMask sourceLayer;
    public Sprite sprite;
    public PlayerColors playerColors;
    public int outputLayer;
    public PhysicsMaterial2D surfaceMaterial;

    [ContextMenu("Process all colliders")]
    void EditColliders()
    {
        var parent = new GameObject("Colliders");
        parent.tag = "Generated Slip-Colliders";
        int i = 0;
        foreach (var c in FindObjectsOfType<BoxCollider2D>())
        {
            if (sourceLayer == (sourceLayer | (1 << c.gameObject.layer)))
            {
                var go = ProcessCollider(c, partitionSize, width, sprite);
                go.name = go.name + i;
                go.transform.SetParent(parent.transform, true);
                i++;
            }
        }
        Undo.RegisterCreatedObjectUndo(parent, "Generated Colliders");
    }

    GameObject ProcessCollider(BoxCollider2D collider, float partitionSize, float width, Sprite sprite)
    {
        GameObject go = new GameObject("Collider");
        go.tag = "Generated Slip-Colliders";
        var corners = WorldSpaceCorners(collider);

        float rotation = collider.transform.rotation.eulerAngles.z + 90f;
        var size = new Vector2(partitionSize, width);

        for (int i = 0; i < corners.Length; i++)
        {
            Vector2 v1 = corners[i], v2 = corners[(i + 1) % corners.Length];
            int count = (int)((v1 - v2).magnitude / partitionSize);
            var positions = PositionsOnLine(v1, v2, count);

            foreach (var p in positions)
            {
                var child = CreateColliderPart(p, rotation, size);
                child.transform.SetParent(go.transform, true);
            }

            rotation = (rotation + 90) % 360;
        }

        // change material of collider
        Undo.RecordObject(collider, "Changed Surface Material");
        collider.sharedMaterial = surfaceMaterial;

        return go;
    }

    GameObject CreateColliderPart(Vector2 position, float rotation, Vector2 size)
    {
        var randHeight = Random.Range(1, 1.6f);

        var go = new GameObject("ColliderPart");
        go.tag = "Generated Slip-Colliders";

        go.layer = outputLayer;
        go.transform.position = position;
        go.transform.localRotation = Quaternion.Euler(0, 0, rotation);
        go.transform.localScale = new Vector3(size.x * 1.2f, size.y * randHeight, 1);

        var renderer = go.AddComponent<SpriteRenderer>();
        var surface = go.AddComponent<SlipperySurface>();
        var collider = go.AddComponent<BoxCollider2D>();

        collider.size = Vector2.one;

        surface.renderer = renderer;
        surface.playerColors = playerColors;

        renderer.sprite = sprite;
        renderer.color = Color.black;
        renderer.sortingOrder = 200;
        renderer.enabled = false;

        return go;
    }

    static Vector2[] WorldSpaceCorners(BoxCollider2D collider)
    {
        var edges = new Vector2[4];
        float x = 0.5f * collider.size.x, y = 0.5f * collider.size.y;
        edges[0] = new Vector2(x, y);
        edges[1] = new Vector2(x, -y);
        edges[2] = new Vector2(-x, -y);
        edges[3] = new Vector2(-x, y);
        for (int i = 0; i < edges.Length; i++)
        {
            edges[i] += collider.offset;
            edges[i] = collider.transform.TransformPoint(edges[i]);
        }
        return edges;
    }

    static Vector2[] PositionsOnLine(Vector2 start, Vector2 end, int count)
    {
        var positions = new Vector2[count];
        float fraction = 1f / (float)count;
        for (int i = 0; i < count; i++)
        {
            var f = fraction * i + fraction * 0.5f;
            var pos = Vector2.Lerp(start, end, f);
            positions[i] = pos;
        }
        return positions;
    }

}
