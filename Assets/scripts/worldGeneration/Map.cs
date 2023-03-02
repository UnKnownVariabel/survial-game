using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour
{
    public Color[] colors;
    public RawImage[] images;
    public float chunkSize = 100f;
    public Transform mapPos;
    public Vector3 offset;

    public static Map instance;

    private void Awake()
    {
        instance = this;
    }

    private void LateUpdate()
    {
        mapPos.localPosition = -(Globals.player.transform.position - offset) * chunkSize / WorldGeneration.chunkSize;

    }

    public void DrawMap()
    {
        
        images[0].texture = ChunkToTexture(Globals.chunks[(Globals.currentChunk.x -1, Globals.currentChunk.y +1)]);
        images[1].texture = ChunkToTexture(Globals.chunks[(Globals.currentChunk.x, Globals.currentChunk.y + 1)]);
        images[2].texture = ChunkToTexture(Globals.chunks[(Globals.currentChunk.x + 1, Globals.currentChunk.y + 1)]);
        images[3].texture = ChunkToTexture(Globals.chunks[(Globals.currentChunk.x - 1, Globals.currentChunk.y)]);
        images[4].texture = ChunkToTexture(Globals.chunks[(Globals.currentChunk.x, Globals.currentChunk.y)]);
        images[5].texture = ChunkToTexture(Globals.chunks[(Globals.currentChunk.x + 1, Globals.currentChunk.y)]);
        images[6].texture = ChunkToTexture(Globals.chunks[(Globals.currentChunk.x - 1, Globals.currentChunk.y - 1)]);
        images[7].texture = ChunkToTexture(Globals.chunks[(Globals.currentChunk.x, Globals.currentChunk.y - 1)]);
        images[8].texture = ChunkToTexture(Globals.chunks[(Globals.currentChunk.x + 1, Globals.currentChunk.y - 1)]);
        offset = new Vector3(Globals.currentChunk.x * WorldGeneration.chunkSize, Globals.currentChunk.y * WorldGeneration.chunkSize);
    }

    public Texture2D ChunkToTexture(Chunk chunk)
    {
        Texture2D texture = new Texture2D(WorldGeneration.chunkSize, WorldGeneration.chunkSize);
        for(int y = 0; y < WorldGeneration.chunkSize; y++)
        {
            for(int x = 0; x < WorldGeneration.chunkSize; x++)
            {
                Color color = colors[chunk.tiles[x, y] % 16];
                texture.SetPixel(x, y, color);
            }
        }
        texture.Apply();
        texture.filterMode = FilterMode.Point;
        return texture;
    }
}
