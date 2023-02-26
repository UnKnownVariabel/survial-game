using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSpawner : MonoBehaviour
{
    public Mob[] mobs;
    public float BaseSpawnTime;
    public float spawnOffsetMax;
    private float timeTillSpawn;

    public void SpawnMob()
    {
        float minDistance = Mathf.Sqrt(Camera.main.orthographicSize * Camera.main.aspect * Camera.main.orthographicSize * Camera.main.aspect + Camera.main.orthographicSize * Camera.main.orthographicSize);
        float distance = minDistance;
        float angle = Random.Range(0, 360);
        Vector2 position = new Vector2(Mathf.Sin(angle) * distance, Mathf.Cos(angle) * distance);
        Instantiate(mobs[0], position, Quaternion.identity);
    }
    private void Update()
    {
        if (Globals.timeHandler.isNight())
        {
            timeTillSpawn -= Time.deltaTime;
            if (timeTillSpawn < 0)
            {
                float spawnTime = BaseSpawnTime / Globals.timeHandler.day;
                timeTillSpawn = spawnTime + Random.Range(-spawnTime * spawnOffsetMax, spawnTime * spawnOffsetMax);
                SpawnMob();
            }
        }
    }

}
