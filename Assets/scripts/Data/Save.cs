using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class Save : MonoBehaviour
{
    public static Save instance;
    public static string loadPath;

    private void Awake()
    {
        instance = this;
    }

    public void SaveGame()
    {
        DateTime start = DateTime.Now;
        Globals.worldData.chunks = new ChunkData[Globals.chunks.Values.Count];
        foreach ((Chunk chunk, int i) in Globals.chunks.Values.Select((value, i) => (value, i)))
        {
            Globals.worldData.chunks[i] = new ChunkData(chunk);
        }

        Globals.worldData.name = WorldData.current_name;
        Globals.worldData.time = Globals.timeHandler.time;
        Globals.worldData.day = Globals.timeHandler.day;

        //convert inventory to inventory information stored in WorldData
        Globals.worldData.inventoryTypes = new int[Inventory.instance.spots.Length];
        Globals.worldData.inventoryAmounts = new int[Inventory.instance.spots.Length];
        for(int i = 0; i < Inventory.instance.spots.Length; i++)
        {
            Globals.worldData.inventoryTypes[i] = ItemHandler.ItemTopIndex(Inventory.instance.spots[i].item);
            Globals.worldData.inventoryAmounts[i] = Inventory.instance.spots[i].amount;
        }

        //Saving mob positions
        Globals.worldData.mobs = new MobData[Globals.mobs.Count];
        for(int i = 0; i < Globals.worldData.mobs.Length; i++)
        {
            Globals.worldData.mobs[i] = new MobData(Globals.mobs[i]);
        }
        Globals.worldData.player = new MobData(Globals.player);
        
        //converting data to json and saving it to file;
        string persistentDataPath = Application.persistentDataPath;
        if(!System.IO.Directory.Exists(persistentDataPath + "/saves"))
        {
            System.IO.Directory.CreateDirectory(persistentDataPath + "/saves");
        }
        string filePath = persistentDataPath + "/saves/" + Globals.worldData.name + ".json";
        string data = JsonUtility.ToJson(Globals.worldData);

        System.IO.File.WriteAllText(filePath, data);
        Debug.Log("saved world in: " + (DateTime.Now - start).TotalMilliseconds.ToString() + " milliseconds");
    }

    public void LoadGame()
    {
        string loadedData = System.IO.File.ReadAllText(loadPath);
        WorldData worldData = JsonUtility.FromJson<WorldData>(loadedData);
        WorldData.current_name = worldData.name;
        WorldGeneration.instance.offset = worldData.offset;
        Globals.worldData = worldData;
        Globals.chunks = new Dictionary<(int, int), Chunk>();

        for (int i = 0; i < worldData.chunks.Length; i++)
        {
            Globals.chunks.Add((Globals.worldData.chunks[i].x, Globals.worldData.chunks[i].y), new Chunk(worldData.chunks[i]));
        }
        Globals.timeHandler.time = worldData.time;
        Globals.timeHandler.day = worldData.day;

        for (int i = 0; i < Inventory.instance.spots.Length; i++)
        {
            Inventory.instance.spots[i].Set(ItemHandler.IndexToItem(worldData.inventoryTypes[i]), worldData.inventoryAmounts[i]);
        }

        // spawning mobs
        for(int i = 0; i < worldData.mobs.Length; i++)
        {
            MobSpawner.instance.SpawnMob(worldData.mobs[i].type, worldData.mobs[i].position, worldData.mobs[i].health);
        }
        Globals.player.SetHealth(worldData.player.health);
        Globals.player.transform.position = worldData.player.position;
    }
}
