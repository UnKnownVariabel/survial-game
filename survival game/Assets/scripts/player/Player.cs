using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : Character
{
    public float pickUpDistance = 0.5f;
    [SerializeField] private InventorySpot[] inventory = new InventorySpot[10];
    private InventorySpot selectedInventorySpot
    {
        get
        {
            return _selectedInventorySpet;
        }
        set
        {
            if(value != null)
            {
                if (_selectedInventorySpet != null)
                {
                    _selectedInventorySpet.selected = false;
                }
                _selectedInventorySpet = value;
                _selectedInventorySpet.selected = true;
            }
            else
            {
                Debug.LogError("value null rejected from SelectedInventorySpot");
            }
        }
    }
    private InventorySpot _selectedInventorySpet;
    private Vector2 direction;

    private void Awake()
    {
        for(int i = 0; i < inventory.Length; i++)
        {
            inventory[i].index = i;
        }
        selectedInventorySpot = inventory[0];
    }

    void Update()
    {
        // movement
        direction = Vector2.zero;
        if (Input.GetKey(KeyCode.W))
        {
            direction.y += 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction.y -= 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction.x += 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction.x -= 1;
        }
        Move(direction);

        if (Input.GetKey(KeyCode.Q))
        {
            PickUpItems();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            int i = selectedInventorySpot.index - 1;
            if(i < 0)
            {
                i = inventory.Length - 1;
            }
            selectedInventorySpot = inventory[i];
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            int i = selectedInventorySpot.index + 1;
            if (i >= inventory.Length)
            {
                i = 0;
            }
            selectedInventorySpot = inventory[i];
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (selectedInventorySpot.item != null)
            {
                if (selectedInventorySpot.item.isPlaceble)
                {
                    Debug.Log("is placeble");
                    Vector3Int mousePos = Globals.wallTilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                    selectedInventorySpot.item.placeItem(mousePos.x, mousePos.y);
                    selectedInventorySpot.RemoveItem();
                }
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            Vector3Int mousePos = Globals.wallTilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            Globals.wallTilemap.SetTile(mousePos, null);
        }
    }
    void PickUpItems()
    {
        PickUpInChunk(Globals.currentChunk);
        if (transform.position.x - Globals.currentChunk.x * WorldGeneration.chunkSize > WorldGeneration.chunkSize / 2 - pickUpDistance)
        {
            PickUpInChunk(Globals.chunks[(Globals.currentChunk.x + 1, Globals.currentChunk.y)]);
        }
        else if (transform.position.x - Globals.currentChunk.x * WorldGeneration.chunkSize < -(WorldGeneration.chunkSize / 2 - pickUpDistance))
        {
            PickUpInChunk(Globals.chunks[(Globals.currentChunk.x - 1, Globals.currentChunk.y)]);
        }
        if (transform.position.y - Globals.currentChunk.y * WorldGeneration.chunkSize > WorldGeneration.chunkSize / 2 - pickUpDistance)
        {
            PickUpInChunk(Globals.chunks[(Globals.currentChunk.x, Globals.currentChunk.y + 1)]);
        }
        else if (transform.position.y - Globals.currentChunk.y * WorldGeneration.chunkSize < -(WorldGeneration.chunkSize / 2 - pickUpDistance))
        {
            PickUpInChunk(Globals.chunks[(Globals.currentChunk.x, Globals.currentChunk.y - 1)]);
        }

        void PickUpInChunk(Chunk chunk)
        {
            foreach (Item item in chunk.items)
            {
                if (Vector2.Distance(item.transform.position, transform.position) < pickUpDistance)
                {
                    for (int i = 0; i < inventory.Length; i++)
                    {
                        if (inventory[i].item == null || (inventory[i].item == item.data && inventory[i].amount < item.data.stackSize))
                        {
                            inventory[i].AddItem(item.data);
                            break;
                        }

                    }
                    chunk.items.Remove(item);
                    Destroy(item.gameObject);
                    break;
                }
            }
        }
    }
}
