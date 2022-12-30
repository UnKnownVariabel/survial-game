using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Tilemaps;

public class Player : MovingObject
{
    public Inventory inventory;
    public SpriteRenderer holdingSprite;
    public float pickUpDistance = 0.5f;
    public Animation toolAnimation;
    public Crafting crafting;
    private Vector2 direction;

    protected override void Awake()
    {
        base.Awake();
        lastSwing = DateTime.Now;
        Globals.player = this;
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
            pivotTransform.localScale = new Vector3(1, -1);
        }
        else
        {
            pivotTransform.localScale = new Vector3(1, 1);
        }
        setDirection(Direction);

        if (Input.GetMouseButton(0))
        {
            double time = (DateTime.Now - lastSwing).TotalSeconds;
            if (inventory.selectedInventorySpot.item != null)
            {
                if (inventory.selectedInventorySpot.item.isTool)
                {
                    ToolData tool = (ToolData)inventory.selectedInventorySpot.item;
                    if (time > tool.swingTime)
                    {
                        attack(tool.damage, damageCollider.transform.localPosition, tool.swingTime);
                    }
                }
                else if(time > baseSwingTime)
                {
                    attack(baseDamage, damageCollider.transform.localPosition, baseSwingTime);
                }
            }
            else if(time > baseSwingTime)
            {
                attack(baseDamage, damageCollider.transform.localPosition);
            }
        }
        else if (Input.GetMouseButton(1) && inventory.selectedInventorySpot.item != null)
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
    protected void attack(float damage, Vector2 extraOffset, float swingTime)
    {
        base.attack(damage, extraOffset);
        toolAnimation["tool"].speed = 1 / swingTime;
        toolAnimation.Rewind("tool");
        toolAnimation.Play("tool");
    }
}
