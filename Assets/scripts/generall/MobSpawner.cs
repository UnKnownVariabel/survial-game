using UnityEngine;

// Mobspawner spawns in mobs att night.
public class MobSpawner : MonoBehaviour
{
    public static MobSpawner instance;

    public Mob[] mobs;
    public float baseSpawnTime;
    public float spawnOffsetMax;

    private float timeTillSpawn;

    // Awake is called when script instance is loaded.
    private void Awake()
    {
        instance = this;
        timeTillSpawn = baseSpawnTime + Random.Range(-baseSpawnTime * spawnOffsetMax, baseSpawnTime * spawnOffsetMax);
    }

   // SpawnMob with no parameter spawns random mob at random position around the player.
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

    // Spawn mob with type parameter spawns mob of type at random place around the player.
    public void SpawnMob(int type)
    {
        float minDistance = Mathf.Sqrt(Camera.main.orthographicSize * Camera.main.aspect * Camera.main.orthographicSize * Camera.main.aspect + Camera.main.orthographicSize * Camera.main.orthographicSize);
        float distance = minDistance * 1.5f;
        float angle = Random.Range(0, 360);
        Vector2 position = (Vector2)Player.instance.transform.position + new Vector2(Mathf.Sin(angle) * distance, Mathf.Cos(angle) * distance);
        Instantiate(mobs[type], position, Quaternion.identity);
    }

    // SpawnMob with thre parameters spawns specified mob at specified position with specified amount of health left.
    public void SpawnMob(int i, Vector2 position, float health)
    {
        Mob mob = Instantiate(mobs[i - 1], position, Quaternion.identity);
        mob.SetHealth(health);
    }

    // Update is called once per frame.
    private void Update()
    {
        if (TimeHandler.instance.isNight())
        {
            timeTillSpawn -= Time.deltaTime;
            if (timeTillSpawn < 0)
            {
                float spawnTime = baseSpawnTime / (TimeHandler.instance.day + 1);
                timeTillSpawn = spawnTime + Random.Range(-spawnTime * spawnOffsetMax, spawnTime * spawnOffsetMax);
                SpawnMob();
            }
        }
    }
}
