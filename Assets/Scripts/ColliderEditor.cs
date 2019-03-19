using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderEditor : MonoBehaviour
{

    public float partitionSize = 0.05f, width = 0.02f;
    public LayerMask sourceLayer;
    public LayerMask outputLayer0, outputLayer1;
    public Sprite sprite;
    public Color color0, color1;

    [ContextMenu("Process all colliders")]
    void EditColliders()
    {
        var parent = new GameObject("Colliders");
        parent.tag = "Generated Slip-Colliders";
        foreach (var c in FindObjectsOfType<BoxCollider2D>())
        {
            if (sourceLayer == c.gameObject.layer || true)
            {
                var go = ProcessCollider(c, partitionSize, width, sprite);
                go.transform.SetParent(parent.transform, true);
            }
        }
    }

    [ContextMenu("Delete all generated colliders")]
    void DeleteColliders()
    {
        foreach (var go in GameObject.FindGameObjectsWithTag("Generated Slip-Colliders"))
        {
            Destroy(go);
        }
    }

    GameObject ProcessCollider(BoxCollider2D collider, float partitionSize, float width, Sprite sprite)
    {
        GameObject go = new GameObject("Collider");
        go.tag = "Generated Slip-Colliders";
        var corners = WorldSpaceCorners(collider);

        float rotation = collider.transform.localRotation.z + 90f;
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

        return go;
    }

    GameObject CreateColliderPart(Vector2 position, float rotation, Vector2 size)
    {
        var randHeight = Random.Range(1, 1.6f);

        var go = new GameObject("ColliderPart");
        go.tag = "Generated Slip-Colliders";

        go.transform.position = position;
        go.transform.localRotation = Quaternion.Euler(0, 0, rotation);
        go.transform.localScale = new Vector3(size.x * 1.25f, size.y * randHeight, 1);

        var collider0 = go.AddComponent<BoxCollider2D>();
        var collider1 = go.AddComponent<BoxCollider2D>();
        var trigger = go.AddComponent<BoxCollider2D>();
        var renderer = go.AddComponent<SpriteRenderer>();
        var surface = go.AddComponent<SlipperySurface>();

        var colliderSize = new Vector2(0.8f, 1 / randHeight);
        collider0.size = colliderSize;
        
        collider1.size = colliderSize;
        trigger.size = colliderSize * 2f;

        trigger.isTrigger = true;

        renderer.sprite = sprite;
        renderer.color = Color.black;
        renderer.sortingOrder = 5;
        renderer.enabled = false;

        surface.collider0 = collider0;
        surface.collider1 = collider1;
        surface.renderer = renderer;
        surface.color0 = color0;
        surface.color1 = color1;

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
