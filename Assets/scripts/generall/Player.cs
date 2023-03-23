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
    public DeathScreen deathScreen;

    private Vector2 direction;
    private bool handsOut = false;

    protected override void Awake()
    {
        base.Awake();
        lastSwing = Time.time;
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
            if (crafting.gameObject.activeSelf)
            {
                crafting.UpdatePotentialValues();
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            inventory.DropItem();
        }

        if(inventory.selectedInventorySpot.item != null)
        {
            handsOut = true;
        }
        else
        {
            handsOut = false;
        }

        Vector2 Direction = ((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position).normalized;
        SetDirection(Direction);

        if (Input.GetMouseButton(0))
        {
            float time = Time.time - lastSwing;
            if (inventory.selectedInventorySpot.item != null)
            {
                if (inventory.selectedInventorySpot.item.isTool)
                {
                    ToolData tool = (ToolData)inventory.selectedInventorySpot.item;
                    if (time > tool.swingTime)
                    {
                        Attack(tool.damage, damageCollider.transform.localPosition, tool.swingTime, tool.knockback, tool.multipliers, tool.layerMask);
                    }
                }
                else if(time > baseSwingTime)
                {
                    Attack(baseDamage, damageCollider.transform.localPosition, baseSwingTime,  baseKnockback, Multipliers.One, layerMask);
                }
            }
            else if(time > baseSwingTime)
            {
                Attack(baseDamage, damageCollider.transform.localPosition, baseSwingTime, baseKnockback, Multipliers.One, layerMask);
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
                    Building building = ((PlacebleItemData)inventory.selectedInventorySpot.item).placeItem(pos);
                    building.chunk = chunk;
                    inventory.selectedInventorySpot.RemoveItem();
                }
            }
            else if (inventory.selectedInventorySpot.item.isEdible)
            {
                if(health < maxHealth)
                {
                    EdibleItem item = (EdibleItem)inventory.selectedInventorySpot.item;
                    AddHealth(item.health);
                    inventory.selectedInventorySpot.RemoveItem();
                }
            }
        }
        base.Update();
    }

    protected override void Anim()
    {
        if (handsOut)
        {
            if (rb.velocity == Vector2.zero)
            {
                spriteRenderer.sprite = sprites[12 + dir * 3];
            }
            else
            {
                animState += Time.deltaTime;
                animState = animState % (frameTime * 3);
                spriteRenderer.sprite = sprites[12 + dir * 3 + Mathf.FloorToInt(animState / frameTime)];
            }
        }
        else
        {
            base.Anim();
        }
    }

    protected override void SetDirection(Vector2 direction)
    {
        if (direction.x < 0)
        {
            pivotTransform.localScale = new Vector3(1, -1);
        }
        else
        {
            pivotTransform.localScale = new Vector3(1, 1);
        }
        base.SetDirection(direction);
        if (dir == 2)
        {
            pivotTransform.localPosition = new Vector3(pivotTransform.localPosition.x, pivotTransform.localPosition.y, 0.1f);
        }
        else
        {
            pivotTransform.localPosition = new Vector3(pivotTransform.localPosition.x, pivotTransform.localPosition.y, -0.1f);

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
                    if (inventory.PickUpItem(item.data))
                    {
                        chunk.items.Remove(item);
                        Destroy(item.gameObject);
                    }
                    break;
                }
            }
        }
    }
    protected void Attack(float damage, Vector2 extraOffset, float swingTime, float knockback, Multipliers multipliers, LayerMask layerMask)
    {
        handsOut = true;
        base.Attack(damage, extraOffset, layerMask, knockback, multipliers);
        toolAnimation["tool"].speed = 1 / swingTime;
        toolAnimation.Rewind("tool");
        toolAnimation.Play("tool");
    }
    protected override void Die()
    {
        deathScreen.gameObject.SetActive(true);
    }
}

