using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Tilemaps;

public class Player : Character
{
    public Inventory inventory;
    public SpriteRenderer holdingSprite;
    public float pickUpDistance = 0.5f;
    public Transform handTransform;
    public BoxCollider2D HandCollider;
    public Animation toolAnimation;
    public Crafting crafting;

    private DateTime lastSwing;
    private Vector2 direction;

    protected override void Awake()
    {
        base.Awake();
        lastSwing = DateTime.Now;
    }

    protected override void Update()
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
        if (Input.GetKeyDown(KeyCode.E))
        {
            crafting.gameObject.SetActive(!crafting.gameObject.activeSelf);
        }

        Vector2 Direction = ((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position).normalized;
        if (Direction.x < 0)
        {
            holdingSprite.flipY = true;
            //holdingSprite.transform.localScale = new Vector3(1, -1);
        }
        else
        {
            holdingSprite.flipY = false;
            //holdingSprite.transform.localScale = new Vector3(1, 1);
        }
        handTransform.right = Direction;
        setDirection(Direction);

        if (Input.GetMouseButton(0))
        {
            if (inventory.selectedInventorySpot.item != null)
            {
                if (inventory.selectedInventorySpot.item.isPlaceble)
                {
                    Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector3Int mousePos = Globals.wallTilemap.WorldToCell(pos);
                    Chunk chunk = Globals.GetChunk(pos);
                    (int x, int y) key = chunk.TilePos(pos);
                    if (!chunk.staticObjects.ContainsKey(key))
                    {
                        ((PlacebleItemData)inventory.selectedInventorySpot.item).placeItem(pos);
                        inventory.selectedInventorySpot.RemoveItem();
                    }
                }
                else if (inventory.selectedInventorySpot.item.isTool)
                {
                    ToolData tool = (ToolData)inventory.selectedInventorySpot.item;
                    if((DateTime.Now - lastSwing).TotalSeconds > tool.swingTime)
                    {
                        lastSwing = DateTime.Now;
                        Vector2 boxPos;
                        if (Direction.x < 0)
                        {
                            toolAnimation["tool_left"].speed = 1 / tool.swingTime;
                            toolAnimation.Rewind("tool_left");
                            toolAnimation.Play("tool_left");
                            boxPos = new Vector2(HandCollider.offset.x, -HandCollider.offset.y) * Direction + (Vector2)HandCollider.transform.position;
                        }
                        else
                        {
                            toolAnimation["tool"].speed = 1 / tool.swingTime;
                            toolAnimation.Rewind("tool");
                            toolAnimation.Play("tool");
                            boxPos = HandCollider.offset * Direction + (Vector2)HandCollider.transform.position;
                        }
                        Collider2D[] enemys = Physics2D.OverlapBoxAll(boxPos, HandCollider.size, handTransform.eulerAngles.z);
                        for (int i = 0; i < enemys.Length; i++)
                        {
                            if (enemys[i].gameObject != gameObject)
                            {
                                if (enemys[i].gameObject.TryGetComponent(out DestructibleObject Object))
                                {
                                    Object.TakeDamage(tool.damage);
                                }
                            }
                        }
                    }
                }
            }
        }
        base.Update();
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
                    if (inventory.pickUpItem(item.data))
                    {
                        chunk.items.Remove(item);
                        Destroy(item.gameObject);
                    }
                    break;
                }
            }
        }
    }
}
