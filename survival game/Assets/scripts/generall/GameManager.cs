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
    public float AmountSpawned;
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
        WorldGeneration.instance.startGame();
        for(int i = 0; i < AmountSpawned; i++)
        {
            Item item = Instantiate(itemPrefab, new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), -1), Quaternion.identity);
            item.data = itemType;
        }
        for(int i = 0; i < itemsToSpawn.Length; i++)
        {
            Item item = Instantiate(itemPrefab, new Vector3( i * 0.5f - itemsToSpawn.Length / 2f, 3, -1), Quaternion.identity);
            item.data = itemsToSpawn[i];
        }
        Path path = Pathfinder.createPath(new Vector3(-5f, -3f, 0), new Vector3(10, 16, 0), 20, 1000);
        for(int i = 0; i < path.nodes.Length - 1; i++)
        {
            Debug.DrawLine(path.nodes[i], path.nodes[i + 1], Color.red, 1000, false);
            Debug.Log(path.nodes[i]);
        }
    }
}
