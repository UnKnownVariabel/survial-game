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
        string persistentDataPath = Application.persistentDataPath;
        string filePath = persistentDataPath + "/worldData.json";
        string data = JsonUtility.ToJson(Globals.worldData);

        System.IO.File.WriteAllText(filePath, data);
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
    }
}
