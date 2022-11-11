using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class WorldGeneration : MonoBehaviour
{
    public static Vector2 worldBounds = new Vector2(30, 30);
    public float MinDistanceBetwenFlags = 4;
    [SerializeField] private TileBase grasTile;
    [SerializeField] private TileBase sandTile;
    [SerializeField] private Image mapImage;
    [SerializeField] private Color grasColor;
    [SerializeField] private Color sandColor;
    [SerializeField] private Color waterColor;
    public Tile stonesTile;
    public Tile treeTile;
    public Tilemap groundMap;
    public Tilemap decorationMap;
    public Transform player;

    
    private Vector2Int worldGroundBorder;
    Vector2Int offset;
    public const int chunkSize = 16;
    public static Vector2Int chunkBuffer;
    public static WorldGeneration instance { get; private set; }
    private Dictionary<(int, int), Chunk> chunks;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("there is more than one worldGeneration");
        }
        else
        {
            instance = this;
        }
        chunks = new Dictionary<(int, int), Chunk>();
        offset = new Vector2Int(Random.Range(-10000, 10000), Random.Range(-10000, 10000));
        chunkBuffer = new Vector2Int(2, 1);
    }
    public void startGame()
    {
        Debug.Log(worldGroundBorder);
        Time.timeScale = 1;
        for(int Y = -chunkBuffer.y; Y <= chunkBuffer.y; Y++)
        {
            for(int X = -chunkBuffer.x; X <= chunkBuffer.x; X++)
            {
                GenerateChunk(X, Y);
            }
        }
        Globals.currentChunk = chunks[(0, 0)];
        //GenerateGroundTiles();
        //spawnFlags();
        //GenerateDecorations();
        //spawnBots();
        //minimapSCR.instance.createFlags();
    }

    private void Update()
    {
        if(player.position.x < Globals.currentChunk.x * chunkSize - chunkSize / 2)
        {
            for (int i = -chunkBuffer.y; i <= chunkBuffer.y; i++)
            {
                GenerateChunk(Globals.currentChunk.x - 1 - chunkBuffer.x, Globals.currentChunk.y + i);
            }
            Globals.currentChunk = chunks[(Globals.currentChunk.x - 1, Globals.currentChunk.y)];
            CheckUnnecessaryChunks();

        }
        else if (player.position.x > Globals.currentChunk.x * chunkSize + chunkSize / 2)
        {
            for (int i = -chunkBuffer.y; i <= chunkBuffer.y; i++)
            {
                GenerateChunk(Globals.currentChunk.x + 1 + chunkBuffer.x, Globals.currentChunk.y + i);
            }
            Globals.currentChunk = chunks[(Globals.currentChunk.x + 1, Globals.currentChunk.y)];
            CheckUnnecessaryChunks();
        }
        if (player.position.y < Globals.currentChunk.y * chunkSize - chunkSize / 2)
        {
            for (int i = -chunkBuffer.x; i <= chunkBuffer.x; i++)
            {
                GenerateChunk(Globals.currentChunk.x + i, Globals.currentChunk.y - 1 - chunkBuffer.y);
            }
            Globals.currentChunk = chunks[(Globals.currentChunk.x, Globals.currentChunk.y - 1)];
            CheckUnnecessaryChunks();
        }
        else if (player.position.y > Globals.currentChunk.y * chunkSize + chunkSize / 2)
        {
            for (int i = -chunkBuffer.x; i <= chunkBuffer.x; i++)
            {
                GenerateChunk(Globals.currentChunk.x + i, Globals.currentChunk.y + 1 + chunkBuffer.y);
            }
            Globals.currentChunk = chunks[(Globals.currentChunk.x, Globals.currentChunk.y + 1)];
            CheckUnnecessaryChunks();
        }
    }

    private void GenerateChunk(int x, int y)
    {
        Chunk chunk;
        int startX = x * chunkSize - chunkSize / 2;
        int startY = y * chunkSize - chunkSize / 2;
        if (chunks.ContainsKey((x, y)))
        {
            chunk = chunks[(x, y)];
            if (chunk.isSpawnd == false)
            {
                RedrawChunk(chunk);
            }
            return;
        }
        float[,] DPS = new float[chunkSize, chunkSize];
        float[,] Speed = new float[chunkSize, chunkSize];
        byte[,] tiles = new byte[chunkSize, chunkSize];
        for (int Y = 0; Y < chunkSize; Y++)
        {
            for(int X = 0; X < chunkSize; X++)
            {
                drawTile(startX + X, startY + Y);
            }
        }
        chunk = new Chunk(x, y, DPS, Speed, tiles);
        chunk.isSpawnd = true;
        chunks.Add((x, y), chunk);

        void RedrawChunk(Chunk chunk)
        {
            for (int Y = 0; Y < chunkSize; Y++)
            {
                for (int X = 0; X < chunkSize; X++)
                {
                    switch(chunk.tiles[X, Y])
                    {
                        case 1:
                            groundMap.SetTile(new Vector3Int(X + startX, Y + startY, 0), sandTile);
                            break;
                        case 2:
                            groundMap.SetTile(new Vector3Int(X + startX, Y + startY, 0), grasTile);
                            break;
                    }
                }
            }
            chunk.isSpawnd = true;
        }

        void drawTile(int x, int y)
        {
            float magnification = 10;
            float noise = Mathf.PerlinNoise((x + offset.x) / magnification, (y + offset.y) / magnification);
            TileData data;
            if (noise > 0.45)
            {
                groundMap.SetTile(new Vector3Int(x, y, 0), grasTile);
                //texture.SetPixel(x, y, grasColor);
                data = TileManager.instance.getData(grasTile);
                tiles[x - startX, y - startY] = 2;

            }
            else if (noise > 0.3)
            {
                groundMap.SetTile(new Vector3Int(x, y, 0), sandTile);
                //texture.SetPixel(x, y, sandColor);
                data = TileManager.instance.getData(sandTile);
                tiles[x - startX, y - startY] = 1;
            }
            else
            {
                //texture.SetPixel(x, y, waterColor);
                data = TileManager.instance.getData(null);
                tiles[x - startX, y - startY] = 0;
            }
            DPS[x - startX, y - startY] = data.DPS;
            Speed[x - startX, y - startY] = data.speed;
        }
    }

    private void CheckUnnecessaryChunks()
    {
        int x = Globals.currentChunk.x;
        int y = Globals.currentChunk.y;

        for(int i = -chunkBuffer.x - 1; i <= chunkBuffer.x + 1; i++)
        {
            CheckChunk(x + i, y - chunkBuffer.y - 1);
            CheckChunk(x + i, y + chunkBuffer.y + 1);
        }

        for (int i = -chunkBuffer.y; i <= chunkBuffer.y; i++)
        {
            CheckChunk(x + chunkBuffer.x + 1, y + i);
            CheckChunk(x - chunkBuffer.x - 1, y + i);
        }

        void CheckChunk(int x, int y)
        {
            if(chunks.ContainsKey((x, y)))
            {
                Chunk chunk = chunks[(x, y)];
                if (chunk.isSpawnd)
                {
                    HideChunk(x, y);
                    chunk.isSpawnd = false;
                }
            }
        }
    }

    private void HideChunk(int x, int y)
    {
        int startX = x * chunkSize - chunkSize / 2;
        int startY = y * chunkSize - chunkSize / 2;
        for (int Y = 0; Y < chunkSize; Y++)
        {
            for (int X = 0; X < chunkSize; X++)
            {
                groundMap.SetTile(new Vector3Int(X + startX, Y + startY, 0), null);
            }
        }
    }

    /*private void GenerateDecorations()
    {
        int amountOfTrees = Mathf.RoundToInt(worldBounds.x * worldBounds.y * 0.02f);
        int amountOfRocks = Mathf.RoundToInt(worldBounds.x * worldBounds.y * 0.1f);
        for (int i = 0; i < amountOfRocks; i++)
        {
            Vector3Int rockPosition = new Vector3Int(Mathf.RoundToInt(Random.Range(-(worldGroundBorder.x -2) / 2, (worldGroundBorder.x - 2) / 2)), Mathf.RoundToInt(Random.Range(-(worldGroundBorder.y - 2) / 2, (worldGroundBorder.y - 2) / 2)), 0);
            if (TileManager.instance.getTile((Vector2Int)rockPosition) != null)
            {
                decorationMap.SetTile(rockPosition, stonesTile);
            }
            
        }
        for (int i = 0; i < amountOfTrees;)
        {
            Vector2Int treePosition = new Vector2Int(Mathf.RoundToInt(Random.Range(-(worldGroundBorder.x - 2) / 2, (worldGroundBorder.x - 2) / 2)), Mathf.RoundToInt(Random.Range(-(worldGroundBorder.y - 2) / 2, (worldGroundBorder.y - 2) / 2)));
            TileBase tile = TileManager.instance.getTile(treePosition);
            if (!checkForFlag(treePosition) && tile != null && TileManager.instance.getData(tile).plantsurviveplantSurvivability >= 0.7)
            {
                Vector3Int position = (Vector3Int)treePosition;
                decorationMap.SetTile(position, treeTile);
                i++;
            }
        }
    }*/
}
