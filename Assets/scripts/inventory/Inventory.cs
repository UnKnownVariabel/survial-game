using UnityEngine;

// Inventory stores and displays all the items the player is holding.
public class Inventory : MonoBehaviour
{
    public SpriteRenderer holdingSprite;
    public Crafting crafting;
    [SerializeField] public InventorySpot[] spots;
    public static Inventory instance;

    // The spot in the inventory which is currently sellected.
    public InventorySpot selectedInventorySpot
    {
        get
        {
            return _selectedInventorySpet;
        }
        set
        {
            if (value != null)
            {
                if (_selectedInventorySpet != null)
                {
                    _selectedInventorySpet.selected = false;
                }
                _selectedInventorySpet = value;
                _selectedInventorySpet.selected = true;
                if(selectedInventorySpot.item != null)
                {
                    try
                    {
                        ToolData toolData = (ToolData)selectedInventorySpot.item;
                        Player.instance.damageCollider.offset = toolData.offset;
                        Player.instance.damageCollider.size = toolData.size;
                    }
                    catch
                    {

                    }
                }
            }
            else
            {
                Debug.LogError("value null rejected from SelectedInventorySpot");
            }
        }
    }
    private InventorySpot _selectedInventorySpet;

    // Start is called before the first frame update.
    private void Awake()
    {
        instance = this;
        for (int i = 0; i < spots.Length; i++)
        {
            spots[i].index = i;
            spots[i].holdingSprite = holdingSprite;
            spots[i].inventory = this;
        }
        selectedInventorySpot = spots[0];
    }

    // Update is called once per frame.
    void Update()
    {
        // Arow navigation in inventory.
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            int i = selectedInventorySpot.index - 1;
            if (i < 0)
            {
                i = spots.Length - 1;
            }
            selectedInventorySpot = spots[i];
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            int i = selectedInventorySpot.index + 1;
            if (i >= spots.Length)
            {
                i = 0;
            }
            selectedInventorySpot = spots[i];
        }

        // Numpad navigation in inventory.
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedInventorySpot = spots[0];
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedInventorySpot = spots[1];
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            selectedInventorySpot = spots[2];
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            selectedInventorySpot = spots[3];
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            selectedInventorySpot = spots[4];
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            selectedInventorySpot = spots[5];
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            selectedInventorySpot = spots[6];
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            selectedInventorySpot = spots[7];
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            selectedInventorySpot = spots[8];
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            selectedInventorySpot = spots[9];
        }
    }

    // PickUpItem places item in inventory if possible.
    public bool PickUpItem(ItemData data)
    {
        for (int i = 0; i < spots.Length; i++)
        {
            if (spots[i].item == data && spots[i].amount < data.stackSize)
            {
                spots[i].AddItem(data);
                CallUpdatePotentialValues();
                return true;
            }
        }
        if (selectedInventorySpot.item == null || (selectedInventorySpot.item == data && selectedInventorySpot.amount < data.stackSize))
        {
            selectedInventorySpot.AddItem(data);
            CallUpdatePotentialValues();
            return true;
        }
        for (int i = 0; i < spots.Length; i++)
        {
            if (spots[i].item == null)
            {
                spots[i].AddItem(data);
                CallUpdatePotentialValues();
                return true;
            }
        }
        return false;
        void CallUpdatePotentialValues()
        {
            if (crafting.gameObject.activeSelf)
            {
                crafting.UpdatePotentialValues();
            }
        }
    }
    
    // Trys to find a inventory spot with the same type of item in it as the paramater and then select it.
    public void TryFindSameItem(ItemData data)
    {
        for(int i = 0; i < spots.Length; i++)
        {
            if (spots[i].item == data && spots[i].amount > 0)
            {
                selectedInventorySpot = spots[i];
                return;
            }
        }
    }

    // Checks if inventory contains enough of the item specified in the ingredient.
    public bool CheckForIngredient(Ingredient ingredient)
    {
        int amount = 0;
        for(int i = 0; i < spots.Length; i++)
        {
            if(ingredient.item == spots[i].item)
            {
                amount += spots[i].amount;
                if(amount >= ingredient.amount)
                {
                    return true;
                }
            }
        }
        return false;
    }

    // Removes items of type and amount specified in ingredient.
    public void RemoveIngredient(Ingredient ingredient)
    {
        for (int i = 0; i < spots.Length; i++)
        {
            if (ingredient.item == spots[i].item)
            {
                if(spots[i].amount >= ingredient.amount)
                {
                    spots[i].amount -= ingredient.amount - 1;
                    spots[i].RemoveItem();
                    return;
                }
                else
                {
                    ingredient.amount -= spots[i].amount;
                    spots[i].amount = 1;
                    spots[i].RemoveItem();
                }
            }
        }
    }

    // Drops item on ground.
    public void DropItem()
    {
        if(selectedInventorySpot.item != null)
        {
            Item item = Instantiate(crafting.itemPrefab, crafting.player.transform.position + new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f)), Quaternion.identity);
            item.data = selectedInventorySpot.item;
            Globals.chunks[(Mathf.RoundToInt(item.transform.position.x / 16), Mathf.RoundToInt(item.transform.position.y / 16))].AddItem(item);

            selectedInventorySpot.RemoveItem();
        }   
    }
}
