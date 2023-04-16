using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// GameManager manages genarall things.
public class GameManager : MonoBehaviour
{
    static private string _playerName;
    static public string playerName
    {
        get 
        {
            if (_playerName == null || _playerName == "")
            {
                _playerName = PlayerPrefs.GetString("name");
            }
            return _playerName; 
        }
        set
        {
            _playerName = value;
            PlayerPrefs.SetString("name", value);
        }
    }
    public Item itemPrefab;
    public ItemData[] itemsToSpawn;
    public Mob mob;
    public float amountOfMobs;
    public Tilemap groundTilemap, decorationTilemap, wallTilemap;
    public MobSpawner mobSpawner;

    [SerializeField] private GameObject pauseMenu;

    // Awake is called when script instance is loaded.
    private void Awake()
    {
        Globals.destructibleObjects = new List<DestructibleObject>();
        Globals.characters = new List<MovingObject>();
        Globals.groundTilemap = groundTilemap;
        Globals.decorationTilemap = decorationTilemap;
        Globals.wallTilemap = wallTilemap;
        Globals.mobs = new List<Mob>();
        Globals.targets = new List<DestructibleObject>();
        Globals.walls = new Dictionary<(int, int), Building>();
    }

    // Start is called before the first frame update.
    private void Start()
    {
        WorldGeneration.instance.StartGame();
        Map.instance.DrawMap();
        if (WorldGeneration.isCreating)
        {
            for (int i = 0; i < itemsToSpawn.Length; i++)
            {
                Item item = Instantiate(itemPrefab, new Vector3(i * 0.5f - itemsToSpawn.Length / 2f, 3, -1), Quaternion.identity);
                item.data = itemsToSpawn[i];
                Globals.chunks[(Mathf.RoundToInt(item.transform.position.x / 16), Mathf.RoundToInt(item.transform.position.y / 16))].AddItem(item);
            }
            for (int i = 0; i < amountOfMobs; i++)
            {
                mobSpawner.SpawnMob();
            }
        }
    }

    // Update is called once per frame.
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
        }
    }
}
