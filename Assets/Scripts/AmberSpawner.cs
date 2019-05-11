using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmberSpawner : MonoBehaviour
{
    public BoxCollider2D spawnArea;
    public float particlesPerSecond;
    public GameObject prefab;
    public float lifeTime;
    public Vector2 verticalSpeed = Vector2.up * 5;
    public float horizontalSpeed = 1;
    public Gradient color;
    public DarknessControl darkness;

    private int particlesSpawned;

    void Update()
    {
        while (particlesSpawned < particlesPerSecond * Time.time)
        {
            SpawnAmber();
        }
    }

    public static Vector2 RandomPointInBounds(Bounds bounds)
    {
        return new Vector2(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y)
        );
    }


    void SpawnAmber()
    {
        Vector2 pos = RandomPointInBounds(spawnArea.bounds);
        var rot = Quaternion.Euler(0, 0, Random.value * 360);
        var amber = Instantiate(prefab, pos, rot);

        Vector2 speed = new Vector2(Mathf.Lerp(horizontalSpeed, -horizontalSpeed, Random.value), Mathf.Lerp(verticalSpeed.x, verticalSpeed.y, Random.value));
        amber.GetComponent<Rigidbody2D>().AddForce(speed, ForceMode2D.Impulse);
        amber.GetComponent<Rigidbody2D>().AddTorque(Random.Range(-1, 1) * 3, ForceMode2D.Impulse);
        amber.GetComponent<SpriteRenderer>().color = color.Evaluate(Random.value);

        darkness.lightSources.Add(amber.transform);

        Destroy(amber, lifeTime);
        particlesSpawned++;
    }

}
