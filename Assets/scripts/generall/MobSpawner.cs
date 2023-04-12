using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MobSpawner : MonoBehaviour
{
    public static MobSpawner instance;

    public Mob[] mobs;
    public float baseSpawnTime;
    public float spawnOffsetMax;

    private float timeTillSpawn;

    private void Awake()
    {
        instance = this;
        timeTillSpawn = baseSpawnTime + Random.Range(-baseSpawnTime * spawnOffsetMax, baseSpawnTime * spawnOffsetMax);
    }

    public void SpawnMob()
    {
        float number = Random.Range(0f, 1f);
        if (number < 0.7)
        {
            SpawnMob(0);
        }
        else
        {
            SpawnMob(1);
        }
    }

    public void SpawnMob(int i)
    {
        float minDistance = Mathf.Sqrt(Camera.main.orthographicSize * Camera.main.aspect * Camera.main.orthographicSize * Camera.main.aspect + Camera.main.orthographicSize * Camera.main.orthographicSize);
        float distance = minDistance * 1.5f;
        float angle = Random.Range(0, 360);
        Vector2 position = (Vector2)Globals.player.transform.position + new Vector2(Mathf.Sin(angle) * distance, Mathf.Cos(angle) * distance);
        Instantiate(mobs[i], position, Quaternion.identity);
    }

    public void SpawnMob(int i, Vector2 position, float health)
    {
        Mob mob = Instantiate(mobs[i - 1], position, Quaternion.identity);
        mob.SetHealth(health);
    }
    private void Update()
    {
        if (Globals.timeHandler.isNight())
        {
            timeTillSpawn -= Time.deltaTime;
            if (timeTillSpawn < 0)
            {
                float spawnTime = baseSpawnTime / Globals.timeHandler.day;
                timeTillSpawn = spawnTime + Random.Range(-spawnTime * spawnOffsetMax, spawnTime * spawnOffsetMax);
                SpawnMob();
            }
        }
    }
}
