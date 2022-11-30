using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public Item itemPrefab;
    public ItemData itemType;
    public ItemData[] itemsToSpawn;
    public Tilemap groundTilemap, decorationTilemap, wallTilemap;
    private void Awake()
    {
        Globals.destructibleObjects = new List<DestructibleObject>();
        Globals.characters = new List<Character>();
        Globals.groundTilemap = groundTilemap;
        Globals.decorationTilemap = decorationTilemap;
        Globals.wallTilemap = wallTilemap;
    }
    private void Start()
    {
        WorldGeneration.worldBounds = new Vector2(16, 16);
        WorldGeneration.instance.startGame();
        for(int i = 0; i < 30; i++)
        {
            Item item = Instantiate(itemPrefab, new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), -1), Quaternion.identity);
            item.data = itemType;
        }
        for(int i = 0; i < itemsToSpawn.Length; i++)
        {
            Item item = Instantiate(itemPrefab, new Vector3( i * 0.5f - itemsToSpawn.Length / 2f, 3, -1), Quaternion.identity);
            item.data = itemsToSpawn[i];
        }
    }
}
