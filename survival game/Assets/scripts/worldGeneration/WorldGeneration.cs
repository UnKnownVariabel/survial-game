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
        offset = new Vector2Int(Random.Range(-100, 100), Random.Range(-100, 100));
    }
    public void startGame()
    {
        Debug.Log("starting game");
        Time.timeScale = 1;
        GenerateChunk(0, 0);
        GenerateChunk(1, 0);
        GenerateChunk(1, 1);
        GenerateChunk(0, 1);
        GenerateChunk(-1, 0);
        GenerateChunk(-1, -1);
        GenerateChunk(0, -1);
        GenerateChunk(1, -1);
        GenerateChunk(-1, 1);
        Globals.currentChunk = chunks[(0, 0)];
        Debug.Log(Globals.currentChunk);

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
            Globals.currentChunk = chunks[(Globals.currentChunk.x - 1, Globals.currentChunk.y)];
        }
        else if (player.position.x > Globals.currentChunk.x * chunkSize + chunkSize / 2)
        {
            Globals.currentChunk = chunks[(Globals.currentChunk.x + 1, Globals.currentChunk.y)];
        }
        if (player.position.y < Globals.currentChunk.y * chunkSize - chunkSize / 2)
        {
            Globals.currentChunk = chunks[(Globals.currentChunk.x, Globals.currentChunk.y - 1)];
        }
        else if (player.position.y > Globals.currentChunk.y * chunkSize + chunkSize / 2)
        {
            Globals.currentChunk = chunks[(Globals.currentChunk.x, Globals.currentChunk.y + 1)];
        }
    }

    private void GenerateChunk(int x, int y)
    {
        int startX = x * chunkSize - chunkSize / 2;
        int startY = y * chunkSize - chunkSize / 2;
        float[,] DPS = new float[chunkSize, chunkSize];
        float[,] Speed = new float[chunkSize, chunkSize];
        for (int Y = 0; Y < chunkSize; Y++)
        {
            for(int X = 0; X < chunkSize; X++)
            {
                drawTile(startX + X, startY + Y);
            }
        }
        Chunk chunk = new Chunk(x, y, DPS, Speed);
        chunks.Add((x, y), chunk);

        void drawTile(int x, int y)
        {
            float magnification = 10;
            float noise = Mathf.PerlinNoise((x + offset.x) / magnification, (y + offset.y) / magnification);
            TileData data;
            if (noise > 0.45)
            {
                groundMap.SetTile(new Vector3Int(x - worldGroundBorder.x / 2, y - worldGroundBorder.y / 2, 0), grasTile);
                //texture.SetPixel(x, y, grasColor);
                data = TileManager.instance.getData(grasTile);

            }
            else if (noise > 0.3)
            {
                groundMap.SetTile(new Vector3Int(x - worldGroundBorder.x / 2, y - worldGroundBorder.y / 2, 0), sandTile);
                //texture.SetPixel(x, y, sandColor);
                data = TileManager.instance.getData(sandTile);
            }
            else
            {
                //texture.SetPixel(x, y, waterColor);
                data = TileManager.instance.getData(null);
            }
            DPS[x - startX, y - startY] = data.DPS;
            Speed[x - startX, y - startY] = data.speed;
        }
    }
    private void GenerateGroundTiles()
    {
        worldGroundBorder = new Vector2Int(Mathf.RoundToInt(worldBounds.x + worldBounds.x % 2), Mathf.RoundToInt(worldBounds.y + worldBounds.y % 2));
        //Texture2D texture = new Texture2D(worldGroundBorder.x,worldGroundBorder.y);
        Pathfinder.tiles = new Pathfinder.node[worldGroundBorder.x, worldGroundBorder.y];
        Pathfinder.GridSizeX = worldGroundBorder.x;
        Pathfinder.GridSizeY = worldGroundBorder.y;
        float[,] DPS = new float[worldGroundBorder.x, worldGroundBorder.y];
        float[,] Speed = new float[worldGroundBorder.x, worldGroundBorder.y];
        for (int y = 0; y < worldGroundBorder.y; y++)
        {
            for (int x = 0; x < worldGroundBorder.x; x++)
            {
                drawTile(x, y, worldGroundBorder.x, worldGroundBorder.y);
            }
        }
        //texture.Apply();
        //mapImage.material.mainTexture = texture;
        TileManager.instance.Init(DPS, Speed);
        void drawTile(int x, int y, int width, int height)
        {
            float magnification = 10;
            float noise = Mathf.PerlinNoise((x + offset.x) / magnification, (y + offset.y) / magnification);
            TileData data;
            if (noise > 0.45)
            {
                groundMap.SetTile(new Vector3Int(x - worldGroundBorder.x / 2, y - worldGroundBorder.y / 2, 0), grasTile);
                //texture.SetPixel(x, y, grasColor);
                data = TileManager.instance.getData(grasTile);
                
            }
            else if (noise > 0.3)
            {
                groundMap.SetTile(new Vector3Int(x - worldGroundBorder.x / 2, y - worldGroundBorder.y / 2, 0), sandTile);
                //texture.SetPixel(x, y, sandColor);
                data = TileManager.instance.getData(sandTile);
            }
            else
            {
                //texture.SetPixel(x, y, waterColor);
                data = TileManager.instance.getData(null);
            }
            Pathfinder.tiles[x, y] = new Pathfinder.node(x, y, data.speed, data.DPS);
            DPS[x, y] = data.DPS;
            Speed[x, y] = data.speed;
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
