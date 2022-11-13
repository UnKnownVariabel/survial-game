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

    Vector2Int offset;
    public const int chunkSize = 16;
    public static Vector2Int chunkBuffer;
    public static WorldGeneration instance { get; private set; }
    private List<Action> actions = new List<Action>();

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
        Globals.chunks = new Dictionary<(int, int), Chunk>();
        offset = new Vector2Int(Random.Range(-10000, 10000), Random.Range(-10000, 10000));
        chunkBuffer = new Vector2Int(2, 1);
    }
    public void startGame()
    {
        Time.timeScale = 1;
        for(int Y = -chunkBuffer.y; Y <= chunkBuffer.y; Y++)
        {
            for(int X = -chunkBuffer.x; X <= chunkBuffer.x; X++)
            {
                GenerateChunk(X, Y);
            }
        }
        Globals.currentChunk = Globals.chunks[(0, 0)];
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
                actions.Add(new Action(0, Globals.currentChunk.x - 1 - chunkBuffer.x, Globals.currentChunk.y + i));
                //GenerateChunk(Globals.currentChunk.x - 1 - chunkBuffer.x, Globals.currentChunk.y + i);
            }
            Globals.currentChunk = Globals.chunks[(Globals.currentChunk.x - 1, Globals.currentChunk.y)];
            CheckUnnecessaryChunks();

        }
        else if (player.position.x > Globals.currentChunk.x * chunkSize + chunkSize / 2)
        {
            for (int i = -chunkBuffer.y; i <= chunkBuffer.y; i++)
            {
                actions.Add(new Action(0, Globals.currentChunk.x + 1 + chunkBuffer.x, Globals.currentChunk.y + i));
                //GenerateChunk(Globals.currentChunk.x + 1 + chunkBuffer.x, Globals.currentChunk.y + i);
            }
            Globals.currentChunk = Globals.chunks[(Globals.currentChunk.x + 1, Globals.currentChunk.y)];
            CheckUnnecessaryChunks();
        }
        if (player.position.y < Globals.currentChunk.y * chunkSize - chunkSize / 2)
        {
            for (int i = -chunkBuffer.x; i <= chunkBuffer.x; i++)
            {
                actions.Add(new Action(0, Globals.currentChunk.x + i, Globals.currentChunk.y - 1 - chunkBuffer.y));
                //GenerateChunk(Globals.currentChunk.x + i, Globals.currentChunk.y - 1 - chunkBuffer.y);
            }
            Globals.currentChunk = Globals.chunks[(Globals.currentChunk.x, Globals.currentChunk.y - 1)];
            CheckUnnecessaryChunks();
        }
        else if (player.position.y > Globals.currentChunk.y * chunkSize + chunkSize / 2)
        {
            for (int i = -chunkBuffer.x; i <= chunkBuffer.x; i++)
            {
                actions.Add(new Action(0, Globals.currentChunk.x + i, Globals.currentChunk.y + 1 + chunkBuffer.y));
                //GenerateChunk(Globals.currentChunk.x + i, Globals.currentChunk.y + 1 + chunkBuffer.y);
            }
            Globals.currentChunk = Globals.chunks[(Globals.currentChunk.x, Globals.currentChunk.y + 1)];
            CheckUnnecessaryChunks();
        }
        CheckForActions();
    }

    private void GenerateChunk(int x, int y)
    {
        Chunk chunk;
        int startX = x * chunkSize - chunkSize / 2;
        int startY = y * chunkSize - chunkSize / 2;
        if (Globals.chunks.ContainsKey((x, y)))
        {
            chunk = Globals.chunks[(x, y)];
            if (chunk.isSpawnd == false)
            {
                RedrawChunk(chunk);
            }
            return;
        }
        float[,] DPS = new float[chunkSize, chunkSize];
        float[,] Speed = new float[chunkSize, chunkSize];
        byte[,] tiles = new byte[chunkSize, chunkSize];
        Random.InitState(offset.x - x - y);
        for (int Y = 0; Y < chunkSize; Y++)
        {
            for(int X = 0; X < chunkSize; X++)
            {
                drawTile(startX + X, startY + Y);
            }
        }
        chunk = new Chunk(x, y, DPS, Speed, tiles);
        chunk.isSpawnd = true;
        Globals.chunks.Add((x, y), chunk);

        void RedrawChunk(Chunk chunk)
        {
            for (int Y = 0; Y < chunkSize; Y++)
            {
                for (int X = 0; X < chunkSize; X++)
                {
                    switch(chunk.tiles[X, Y] % 16)
                    {
                        case 1:
                            groundMap.SetTile(new Vector3Int(X + startX, Y + startY, 0), sandTile);
                            break;
                        case 2:
                            groundMap.SetTile(new Vector3Int(X + startX, Y + startY, 0), grasTile);
                            break;
                    }
                    switch ((chunk.tiles[X, Y] - chunk.tiles[X, Y] % 16) / 16)
                    {
                        case 1:
                            decorationMap.SetTile(new Vector3Int(X + startX, Y + startY, 0), treeTile);
                            break;
                        case 2:
                            decorationMap.SetTile(new Vector3Int(X + startX, Y + startY, 0), stonesTile);
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
                if (Random.value * data.plantsurviveplantSurvivability < 0.3f)
                {
                    decorationMap.SetTile(new Vector3Int(x, y, 0), treeTile);
                    tiles[x - startX, y - startY] += 16;
                }
                else if(Random.value < 0.1f)
                {
                    decorationMap.SetTile(new Vector3Int(x, y, 0), stonesTile);
                    tiles[x - startX, y - startY] += 32;
                }
                
            }
            else if (noise > 0.3)
            {
                groundMap.SetTile(new Vector3Int(x, y, 0), sandTile);
                //texture.SetPixel(x, y, sandColor);
                data = TileManager.instance.getData(sandTile);
                tiles[x - startX, y - startY] = 1;
                if (Random.value < 0.1f)
                {
                    decorationMap.SetTile(new Vector3Int(x, y, 0), stonesTile);
                    tiles[x - startX, y - startY] += 32;
                }
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
            if(Globals.chunks.ContainsKey((x, y)))
            {
                Chunk chunk = Globals.chunks[(x, y)];
                if (chunk.isSpawnd)
                {
                    actions.Add(new Action(1, x, y));
                    chunk.isSpawnd = false;
                }
            }
        }
    }

    private void CheckForActions()
    {
        if(actions.Count > 0)
        {
            Action action = actions[0];
            actions.RemoveAt(0);
            switch (action.type)
            {
                case 0:
                    GenerateChunk(action.x, action.y);
                    break;
                case 1:
                    HideChunk(action.x, action.y);
                    break;
                default:
                    Debug.Log("this action type dose not exsist: " + action.type.ToString());
                    break;
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
                decorationMap.SetTile(new Vector3Int(X + startX, Y + startY, 0), null);
            }
        }
    }
}
