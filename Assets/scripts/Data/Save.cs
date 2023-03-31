using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class Save : MonoBehaviour
{
    public static Save instance;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {     
        string persistentDataPath = Application.persistentDataPath;
        string filePath = persistentDataPath + "/data.txt";
        string data = "Hello, world! saved data";

        // Write data to a file in the persistent data path
        //System.IO.File.WriteAllText(filePath, data);

        // Read data from a file in the persistent data path
        string loadedData = System.IO.File.ReadAllText(filePath);
        Debug.Log(loadedData);
        Test();


    }

    private void Test()
    {
        string persistentDataPath = Application.persistentDataPath;
        string filePath = persistentDataPath + "/testData.json";

        WorldData worldData = new WorldData();
        worldData.time = 0;
        worldData.day = 0;
        worldData.offset = new Vector2Int(3, 5);
        worldData.name = "test123";

        string data = JsonUtility.ToJson(worldData);
        System.IO.File.WriteAllText(filePath, data);

        string loadedData = System.IO.File.ReadAllText(filePath);
        WorldData worldData1 = JsonUtility.FromJson<WorldData>(loadedData);
        Debug.Log(worldData1.name);
    }

    public void SaveGame()
    {
        Globals.worldData.chunks = new ChunkData[Globals.chunks.Values.Count];
        int i = 0;
        foreach (Chunk chunk in Globals.chunks.Values)
        {
            Globals.worldData.chunks[i] = new ChunkData(chunk);
            i++;
        }
        Globals.worldData.time = Globals.timeHandler.time;
        Globals.worldData.day = Globals.timeHandler.day;

        //convert inventory to inventory information stored in WorldData
        Globals.worldData.inventoryTypes = new int[Inventory.instance.spots.Length];
        Globals.worldData.inventoryAmounts = new int[Inventory.instance.spots.Length];
        for(i = 0; i < Inventory.instance.spots.Length; i++)
        {
            Globals.worldData.inventoryTypes[i] = ItemHandler.ItemTopIndex(Inventory.instance.spots[i].item);
            Globals.worldData.inventoryAmounts[i] = Inventory.instance.spots[i].amount;
        }

        string persistentDataPath = Application.persistentDataPath;
        string filePath = persistentDataPath + "/worldData.json";
        string data = JsonUtility.ToJson(Globals.worldData);

        System.IO.File.WriteAllText(filePath, data);
        Debug.Log("saved world");
    }

    public void LoadGame()
    {
        string persistentDataPath = Application.persistentDataPath;
        string filePath = persistentDataPath + "/worldData.json";
        Debug.Log(filePath);

        string loadedData = System.IO.File.ReadAllText(filePath);
        WorldData worldData = JsonUtility.FromJson<WorldData>(loadedData);
        WorldGeneration.instance.offset = worldData.offset;
        Globals.worldData = worldData;
        Globals.chunks = new Dictionary<(int, int), Chunk>();
        Debug.Log(worldData.chunks.Length);
        for (int i = 0; i < worldData.chunks.Length; i++)
        {
            Globals.chunks.Add((Globals.worldData.chunks[i].x, Globals.worldData.chunks[i].y), new Chunk(worldData.chunks[i]));
        }
        Globals.timeHandler.time = worldData.time;
        Globals.timeHandler.day = worldData.day;

        Debug.Log(worldData.inventoryAmounts);
        for (int i = 0; i < Inventory.instance.spots.Length; i++)
        {
            Inventory.instance.spots[i].Set(ItemHandler.IndexToItem(worldData.inventoryTypes[i]), worldData.inventoryAmounts[i]);
        }
    }
}
