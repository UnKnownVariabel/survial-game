using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Item itemPrefab;
    public ItemData log;
    private void Awake()
    {
        Globals.destructibleObjects = new List<DestructibleObject>();
        Globals.characters = new List<Character>();
    }
    private void Start()
    {
        WorldGeneration.worldBounds = new Vector2(16, 16);
        WorldGeneration.instance.startGame();
        for(int i = 0; i < 20; i++)
        {
            Item item = Instantiate(itemPrefab, new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), -1), Quaternion.identity);
            item.data = log;
        }
    }
}
