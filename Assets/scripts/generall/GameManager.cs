using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public Item itemPrefab;
    public ItemData itemType;
    public ItemData[] itemsToSpawn;
    public Mob mob;
    public float amountOfMobs;
    public Tilemap groundTilemap, decorationTilemap, wallTilemap;
    public float amountSpawned;
    public MobSpawner mobSpawner;

    [SerializeField] private GameObject pauseMenu;
    private void Awake()
    {
        Globals.destructibleObjects = new List<DestructibleObject>();
        Globals.characters = new List<MovingObject>();
        Globals.groundTilemap = groundTilemap;
        Globals.decorationTilemap = decorationTilemap;
        Globals.wallTilemap = wallTilemap;
        Globals.mobs = new List<Mob>();
        Globals.targets = new List<DestructibleObject>();
        Globals.walls = new Dictionary<(int, int), Building>();
    }
    private void Start()
    {
        WorldGeneration.instance.StartGame();
        Map.instance.DrawMap();
        if (WorldGeneration.isCreating)
        {
            for (int i = 0; i < amountSpawned; i++)
            {
                Item item = Instantiate(itemPrefab, new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), -1), Quaternion.identity);
                item.data = itemType;
            }
            for (int i = 0; i < itemsToSpawn.Length; i++)
            {
                Item item = Instantiate(itemPrefab, new Vector3(i * 0.5f - itemsToSpawn.Length / 2f, 3, -1), Quaternion.identity);
                item.data = itemsToSpawn[i];
                Globals.chunks[(Mathf.RoundToInt(item.transform.position.x / 16), Mathf.RoundToInt(item.transform.position.y / 16))].AddItem(item);
            }
            for (int i = 0; i < amountOfMobs; i++)
            {
                mobSpawner.SpawnMob();
            }
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
        }
    }
}
