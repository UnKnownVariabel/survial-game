using UnityEngine;

// Player is the class attached to the player.
public class Player : MovingObject
{
    public static Player instance;

    [SerializeField] private Inventory inventory;
    [SerializeField] private SpriteRenderer holdingSprite;
    [SerializeField] private float pickUpDistance = 0.5f;
    [SerializeField] private Animation toolAnimation;
    [SerializeField] private Crafting crafting;
    [SerializeField] private DeathScreen deathScreen;
    [SerializeField] private Transform previewPos;
    [SerializeField] private SpriteRenderer previewSprite;
    [SerializeField] private Color cantPlaceColor;
    [SerializeField] private Color canPlaceColor;
    [SerializeField] private ParticleSystem heartEmitter;
    [SerializeField] private float regenePS;


    private Vector2 direction;
    private bool handsOut = false;

    // Awake is called when script instance is loaded.
    protected override void Awake()
    {
        base.Awake();
        lastSwing = Time.time;
        instance = this;
    }

    // Update is called once per frame.
    protected override void Update()
    {
        if (Globals.pause)
        {
            return;
        }

        // Player movement.
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

        // Pickup items.
        if (Input.GetKey(KeyCode.Q))
        {
            PickUpItems();
        }

        // Open / close crafting menu.
        if (Input.GetKeyDown(KeyCode.E))
        {
            crafting.gameObject.SetActive(!crafting.gameObject.activeSelf);
            if (crafting.gameObject.activeSelf)
            {
                crafting.UpdatePotentialValues();
            }
        }

        // Drop item.
        if (Input.GetKeyDown(KeyCode.R))
        {
            inventory.DropItem();
        }

        // Hold out hands if player is holding something.
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

        // Attack if left mouse button is pressed.
        if (Input.GetMouseButton(0) && !MouseInputUIBlocker.BlockedByUI)
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

        // If rigth mouse button pressed either place item if thats possible or eat item if that is possible.
        else if (Input.GetMouseButton(1) && inventory.selectedInventorySpot.item != null && !MouseInputUIBlocker.BlockedByUI)
        {
            if (inventory.selectedInventorySpot.item.isPlaceble)
            {
                Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int mousePos = Globals.wallTilemap.WorldToCell(pos);
                Chunk chunk = Globals.GetChunk(pos);
                (int x, int y) key = chunk.TilePos(pos);
                if (CanPlace(key, chunk))
                {
                    Building building = ((PlacebleItemData)inventory.selectedInventorySpot.item).placeItem(pos);
                    building.chunk = chunk;
                    inventory.selectedInventorySpot.RemoveItem();
                }
            }
            else if (inventory.selectedInventorySpot.item.isEdible)
            {
                if(health < maxHealth && Input.GetMouseButtonDown(1))
                {
                    EdibleItem item = (EdibleItem)inventory.selectedInventorySpot.item;
                    AddHealth(item.health);
                    inventory.selectedInventorySpot.RemoveItem();
                    heartEmitter.Play();
                    soundEffectHandler.PlayClip(4);
                }
            }
        }

        // Preview buildings.
        if(inventory.selectedInventorySpot.item != null)
        {
            if (inventory.selectedInventorySpot.item.isPlaceble)
            {
                Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                previewPos.position = Globals.wallTilemap.WorldToCell(pos) + new Vector3(0.5f, 0.5f, 0f);
                Chunk chunk = Globals.GetChunk(pos);
                (int x, int y) key = chunk.TilePos(pos);
                if (CanPlace(key, chunk))
                {
                    previewSprite.color = canPlaceColor;
                }
                else
                {
                    previewSprite.color = cantPlaceColor;
                }
                previewSprite.sprite = inventory.selectedInventorySpot.item.sprite;
                previewSprite.enabled = true;
            }
            else
            {
                previewSprite.enabled = false;
            }
        }
        else
        {
            previewSprite.enabled = false;
        }

        AddHealth(regenePS * Time.deltaTime);
        base.Update();
    }

    // Checks if building can be placed.
    private bool CanPlace((int x, int y) key, Chunk chunk)
    {
        return !chunk.staticObjects.ContainsKey(key) && chunk.tiles[key.x, key.y] % 16 != 0;
    }

    // Override anim to add all the sprites where the player is holding out his hands.
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

    // Overides SetDirection so tool flips when turning around.
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

    // PickupItems picks upp items which are closer than pickUpDistance.
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
                if(item == null) { Debug.Log(item); }
                if (Vector2.Distance(item.transform.position, transform.position) < pickUpDistance)
                {
                    if (inventory.PickUpItem(item.data))
                    {
                        chunk.RemoveItem(item);
                        Destroy(item.gameObject);
                    }
                    break;
                }
            }
        }
    }

    // Override to attack which calls tool animation.
    protected void Attack(float damage, Vector2 extraOffset, float swingTime, float knockback, Multipliers multipliers, LayerMask layerMask)
    {
        handsOut = true;
        int? type = base.Attack(damage, extraOffset, layerMask, knockback, multipliers);
        if(type!= null)
        {
            soundEffectHandler.PlayClip((int)type + 1);
        }
        toolAnimation["tool"].speed = 1 / swingTime;
        toolAnimation.Rewind("tool");
        toolAnimation.Play("tool");
    }

    // Override to Die which activates the deathscreen when the player dies.
    protected override void Die()
    {
        deathScreen.gameObject.SetActive(true);
    }
}

