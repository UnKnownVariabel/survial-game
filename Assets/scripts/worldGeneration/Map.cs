using UnityEngine;
using UnityEngine.UI;

// Map is attached to the map of the player sorroundings.
public class Map : MonoBehaviour
{
    public Color[] colors;
    public RawImage[] images;
    public float chunkSize = 100f;
    public Transform mapPos;
    public Vector3 offset;
    public Transform point;

    public static Map instance;

    // Awake is called when script instance is loaded.
    private void Awake()
    {
        instance = this;
    }

    // LateUpdate is called after Update.
    private void LateUpdate()
    {
        mapPos.localPosition = -(Player.instance.transform.position - offset) * chunkSize / WorldGeneration.chunkSize;
        point.eulerAngles = Player.instance.pivotTransform.eulerAngles + new Vector3(0, 0, -90);
    }

    // Draws entire map.
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

    // Draws one chunk to one texture.
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
        foreach((int x, int y) key in chunk.buildings.Keys)
        {
            texture.SetPixel(key.x, key.y, colors[3]);
        }
        texture.Apply();
        texture.filterMode = FilterMode.Point;
        return texture;
    }
}
