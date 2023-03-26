using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class WorldGeneration : MonoBehaviour
{
    [SerializeField] private TileBase grasTile;
    [SerializeField] private TileBase sandTile;
    [SerializeField] private Image mapImage;
    [SerializeField] private Color grasColor;
    [SerializeField] private Color sandColor;
    [SerializeField] private Color waterColor;
    public Tile stonesTile;
    public Tile treeTile;
    public StaticObject treePrefab;
    public StaticObject stumpPrefab;
    public StaticObject crossbowPrefab;
    public StaticObject stonePrefab;
    public PlacebleItemData wall;
    public Tilemap groundMap;
    public Tilemap decorationMap;
    public Transform player;

    Vector2Int offset;
    public const int chunkSize = 16;
    public static Vector2Int chunkBuffer;
    public static WorldGeneration instance { get; private set; }
    public List<Action> actions = new List<Action>();

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
    public void StartGame()
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
    }

    private void Update()
    {
        if(player.position.x < Globals.currentChunk.x * chunkSize - chunkSize / 2)
        {
            for (int i = -chunkBuffer.y; i <= chunkBuffer.y; i++)
            {
                actions.Add(new Action(0, Globals.currentChunk.x - 1 - chunkBuffer.x, Globals.currentChunk.y + i));
            }
            Globals.currentChunk = Globals.chunks[(Globals.currentChunk.x - 1, Globals.currentChunk.y)];
            CheckUnnecessaryChunks();
            actions.Add(new Action(2));
        }
        else if (player.position.x > Globals.currentChunk.x * chunkSize + chunkSize / 2)
        {
            for (int i = -chunkBuffer.y; i <= chunkBuffer.y; i++)
            {
                actions.Add(new Action(0, Globals.currentChunk.x + 1 + chunkBuffer.x, Globals.currentChunk.y + i));
            }
            Globals.currentChunk = Globals.chunks[(Globals.currentChunk.x + 1, Globals.currentChunk.y)];
            CheckUnnecessaryChunks();
            actions.Add(new Action(2));
        }
        if (player.position.y < Globals.currentChunk.y * chunkSize - chunkSize / 2)
        {
            for (int i = -chunkBuffer.x; i <= chunkBuffer.x; i++)
            {
                actions.Add(new Action(0, Globals.currentChunk.x + i, Globals.currentChunk.y - 1 - chunkBuffer.y));
            }
            Globals.currentChunk = Globals.chunks[(Globals.currentChunk.x, Globals.currentChunk.y - 1)];
            CheckUnnecessaryChunks();
            actions.Add(new Action(2));
        }
        else if (player.position.y > Globals.currentChunk.y * chunkSize + chunkSize / 2)
        {
            for (int i = -chunkBuffer.x; i <= chunkBuffer.x; i++)
            {
                actions.Add(new Action(0, Globals.currentChunk.x + i, Globals.currentChunk.y + 1 + chunkBuffer.y));
            }
            Globals.currentChunk = Globals.chunks[(Globals.currentChunk.x, Globals.currentChunk.y + 1)];
            CheckUnnecessaryChunks();
            actions.Add(new Action(2));
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
        chunk = new Chunk(x, y, DPS, Speed, tiles);
        for (int Y = 0; Y < chunkSize; Y++)
        {
            for(int X = 0; X < chunkSize; X++)
            {
                DrawTile(startX + X, startY + Y);
            }
        }
        chunk.isSpawnd = true;
        Globals.chunks.Add((x, y), chunk);
        chunk.GenerateNodes();

        void RedrawChunk(Chunk chunk)
        {
            for (int Y = 0; Y < chunkSize; Y++)
            {
                for (int X = 0; X < chunkSize; X++)
                {
                    //checking left half of byte ground tile data
                    switch(chunk.tiles[X, Y] % 16)
                    {
                        case 1:
                            groundMap.SetTile(new Vector3Int(X + startX, Y + startY, 0), sandTile);
                            break;
                        case 2:
                            groundMap.SetTile(new Vector3Int(X + startX, Y + startY, 0), grasTile);
                            break;
                    }
                    //checking right half of byte decoration tile data
                    switch ((chunk.tiles[X, Y] - chunk.tiles[X, Y] % 16) / 16)
                    {
                        case 1:
                            StaticObject tree = Instantiate(treePrefab, new Vector3(X + startX + 0.5f, Y + startY + 0.5f, 0), Quaternion.identity);
                            tree.chunk = chunk;
                            break;
                        case 2:
                            decorationMap.SetTile(new Vector3Int(X + startX, Y + startY, 0), stonesTile);
                            break;
                        case 3:
                            StaticObject stump = Instantiate(stumpPrefab, new Vector3(X + startX + 0.5f, Y + startY + 0.5f, 0), Quaternion.identity);
                            stump.chunk = chunk;
                            break;
                        case 4:
                            wall.placeItem(new Vector3(X + startX + 0.5f, Y + startY + 0.5f, 0));
                            break;
                        case 5:
                            StaticObject crossbow = Instantiate(crossbowPrefab, new Vector3(X + startX + 0.5f, Y + startY + 0.5f, 0), Quaternion.identity);
                            crossbow.chunk = chunk;
                            break;
                        case 6:
                            StaticObject stone = Instantiate(stonePrefab, new Vector3(X + startX + 0.5f, Y + startY + 0.5f, 0), Quaternion.identity);
                            stone.chunk = chunk;
                            break;
                    }
                }
            }
            chunk.isSpawnd = true;
        }

        void DrawTile(int x, int y)
        {
            float magnification = 10;
            float noise = Mathf.PerlinNoise((x + offset.x) / magnification, (y + offset.y) / magnification);
            TileData data;
            if (noise > 0.45)
            {
                groundMap.SetTile(new Vector3Int(x, y, 0), grasTile);
                data = TileManager.instance.GetData(grasTile);
                //setting byte to 00000010 to indicate gras for redrawing chunk
                tiles[x - startX, y - startY] = 2;
                if (Random.value * data.plantSurvivability < 0.3f)
                {
                    StaticObject tree = Instantiate(treePrefab, new Vector3(x + 0.5f, y + 0.5f, 0), Quaternion.identity);
                    tree.chunk = chunk;
                    //adding 00010000 to byte to indicate tree for redrawing chunk
                    //tiles[x - startX, y - startY] += 16;
                }
                else if(Random.value < 0.3f)
                {
                    StaticObject stone = Instantiate(stonePrefab, new Vector3(x + 0.5f, y + 0.5f, 0), Quaternion.identity);
                    stone.chunk = chunk;
                }
                else if(Random.value < 0.1f)
                {
                    decorationMap.SetTile(new Vector3Int(x, y, 0), stonesTile);
                    //adding 00100000 to byte to indicate stone for redrawing chunk
                    //tiles[x - startX, y - startY] += 32;
                }
                
            }
            else if (noise > 0.3)
            {
                groundMap.SetTile(new Vector3Int(x, y, 0), sandTile);
                data = TileManager.instance.GetData(sandTile);
                //setting byte to 00000001 to indicate sand for redrawing chunk
                tiles[x - startX, y - startY] = 1;
                if (Random.value < 0.1f)
                {
                    decorationMap.SetTile(new Vector3Int(x, y, 0), stonesTile);
                    //adding 00100000 to byte to indicate stone for redrawing chunk
                    tiles[x - startX, y - startY] += 32;
                }
            }
            else
            {
                data = TileManager.instance.GetData(null);
                //setting byte to 00000000 to indicate water for redrawing chunk
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
                case 2:
                    Map.instance.DrawMap();
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
        foreach(StaticObject staticObject in Globals.chunks[(x, y)].staticObjects.Values)
        {
            //Debug.Log(tree.transform.position);
            Destroy(staticObject.gameObject);
        }
        Globals.chunks[(x, y)].staticObjects = new Dictionary<(int x, int y), StaticObject>();
    }
}
