using System;
using UnityEngine;

// ChunkData contains all the data needed to save and reconstruct one chunk.
[Serializable]
public class ChunkData
{
    public int x, y;
    public byte[] tiles;
    public int[] items;
    public Vector2[] itemPositions;

    // Constructor converts normal instance of Chunk class to ChunkData.
    public ChunkData (Chunk chunk)
    {
        x = chunk.x;
        y = chunk.y;
        tiles = new byte[chunk.tiles.Length];
        for (int Y = 0; Y < chunk.tiles.GetLength(1); Y++)
        {
            for (int X = 0; X < chunk.tiles.GetLength(0); X++)
            {
                tiles[Y * chunk.tiles.GetLength(0) + X] = chunk.tiles[X, Y];
            }
        }
        items = new int[chunk.items.Count];
        itemPositions = new Vector2[chunk.items.Count];
        int i = 0;
        foreach(Item item in chunk.items)
        {
            items[i] = ItemHandler.ItemTopIndex(item.data);
            itemPositions[i] = item.transform.position;
        }
        itemPositions = chunk.itemPositions.ToArray();
        items = chunk.itemIndexes.ToArray();
    }
}
