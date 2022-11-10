using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class WorldGeneration : MonoBehaviour
{
    public static Vector2 worldBounds = new Vector2(30, 30);
    public float MinDistanceBetwenFlags = 4;
    public List<Vector2Int> flags;
    public static int amountOfFlags = 0;
    public GameObject flagPref;
    public static int amountOfBots = 0;
    public GameObject botPref;
    public List<Color> teamColors;
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
    private Vector2Int worldGroundBorder;
    public static WorldGeneration instance { get; private set; }
    private string[] names = {"Liam", "Noah", "Oliver", "Elijah", "William", "James", "Benjamin", "Lucas", "Henry", "Alexander", "Mason", "Michael", "Ethan", "Daniel", "Jacob", "Logan", "Jackson", "Levi", "Sebastian", "Mateo", "Jack", "Owen", "Theodore", "Aiden", "Samuel", "Joseph", "John", "David", "Wyatt", "Matthew", "Luke", "Asher", "Carter", "Julian", "Grayson", "Leo", "Jayden", "Gabriel", "Isaac", "Lincoln", "Anthony", "Hudson", "Dylan", "Ezra", "Thomas", "Charles", "Jaxon", "Maverick", "Josiah", "Isaiah", "Andrew", "Elias", "Joshua", "Nathan", "Caleb", "Ryan", "Adrian", "Miles", "Eli", "Nolan", "Christian", "Aaron", "Cameron", "Ezekiel", "Colton", "Luca", "Landon", "Hunter", "Jonathan", "Santiago", "Axel", "Easton", "Cooper", "Jeremiah", "Angel", "Roman", "Connor", "Jameson", "Robert", "Greyson", "Jordan", "Ian", "Carson", "Jaxson", "Leonardo", "Nicholas", "Dominic", "Austin", "Everett", "Brooks", "Xavier", "Kai", "Jose", "Parker", "Adam", "Jace", "Wesley", "Kayden", "Silas", "Olivia", "Emma", "Ava", "Charlotte", "Sophia", "Amelia", "Isabella", "Mia", "Evelyn", "Harper", "Camila", "Gianna", "Abigail", "Luna", "Ella", "Elizabeth", "Sofia", "Emily", "Avery", "Mila", "Scarlett", "Eleanor", "Madison", "Layla", "Penelope", "Aria", "Chloe", "Grace", "Ellie", "Nora", "Hazel", "Zoey", "Riley", "Victoria", "Lily", "Aurora", "Violet", "Nova", "Hannah", "Emilia", "Zoe", "Stella", "Everly", "Isla", "Leah", "Lillian", "Addison", "Willow", "Lucy", "Paisley", "Natalie", "Naomi", "Eliana", "Brooklyn", "Elena", "Aubrey", "Claire", "Ivy", "Kinsley", "Audrey", "Maya", "Genesis", "Skylar", "Bella", "Aaliyah", "Madelyn", "Savannah", "Anna", "Delilah", "Serenity", "Caroline", "Kennedy", "Valentina", "Ruby", "Sophie", "Alice", "Gabriella", "Sadie", "Ariana", "Allison", "Hailey", "Autumn", "Nevaeh", "Natalia", "Quinn", "Josephine", "Sarah", "Cora", "Emery", "Samantha", "Piper", "Leilani", "Eva", "Everleigh", "Madeline", "Lydia", "Jade", "Peyton", "Brielle", "Adeline"};
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
    }
    // Start is called before the first frame update
    void Start()
    {
    }
    public void startGame()
    {
        Time.timeScale = 1;
        GenerateGroundTiles();
        //spawnFlags();
        //GenerateDecorations();
        //spawnBots();
        //minimapSCR.instance.createFlags();
    }
    /*private void spawnBots()
    {
        int i = 0;
        for(int I = 0; i < amountOfBots && I < 10000; I++)
        {
            Vector3 position = new Vector3(Random.Range(-worldBounds.x / 2, worldBounds.x / 2), Random.Range(-worldBounds.y / 2, worldBounds.y / 2), -2);
            if (TileManager.instance.getTile(position) != null)
            {
                GameObject bot = Instantiate(botPref, position, Quaternion.identity);
                mainCharacterSCR mainCharacter = bot.GetComponent<mainCharacterSCR>();
                if (teamColors.Count > i)
                {
                    mainCharacter.playerColor = teamColors[i];
                }
                else
                {
                    mainCharacter.playerColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
                }
                mainCharacter.Name = names[Random.Range(0, names.Length)];
                i++;
            }
        }
    }*/
    /*private void spawnFlags()
    {
        for(int i = 0; i < 10000 && flags.Count < amountOfFlags; i++)
        {
            Vector2Int cords = new Vector2Int(Mathf.RoundToInt(Random.Range(-worldBounds.x / 2, worldBounds.x / 2)), Mathf.RoundToInt(Random.Range(-worldBounds.y / 2, worldBounds.y / 2)));
            bool addCords = true;
            if(TileManager.instance.getTile(cords) != null)
            {
                foreach (Vector2 flag in flags)
                {
                    if (Vector2.Distance(cords, flag) <= MinDistanceBetwenFlags)
                    {
                        addCords = false;
                        break;
                    }
                }
            }
            else
            {
                addCords = false;
            }
            if (addCords)
            {
                flags.Add(cords);
            }
        }
        foreach(Vector2 flag in flags)
        {
            Instantiate(flagPref, new Vector3(flag.x, flag.y, -1f), Quaternion.identity);
        }
        
    }*/
    private void GenerateGroundTiles()
    {
        worldGroundBorder = new Vector2Int(Mathf.RoundToInt(worldBounds.x + worldBounds.x % 2), Mathf.RoundToInt(worldBounds.y + worldBounds.y % 2));
        //Texture2D texture = new Texture2D(worldGroundBorder.x,worldGroundBorder.y);
        Pathfinder.tiles = new Pathfinder.node[worldGroundBorder.x, worldGroundBorder.y];
        Pathfinder.GridSizeX = worldGroundBorder.x;
        Pathfinder.GridSizeY = worldGroundBorder.y;
        float[,] DPS = new float[worldGroundBorder.x, worldGroundBorder.y];
        float[,] Speed = new float[worldGroundBorder.x, worldGroundBorder.y];
        Vector2Int offset = new Vector2Int(Random.Range(-100, 100), Random.Range(-100, 100));
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
    /*private bool checkForFlag(Vector2Int position)
    {
        foreach(Vector2Int flag in flags)
        {
            if(flag == position)
            {
                return true;
            }
        }
        return false;
    }*/
}
