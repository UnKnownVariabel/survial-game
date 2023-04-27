using UnityEngine;
using UnityEngine.UI;

public class InventorySpot : MonoBehaviour
{
    public ItemData item;
    public int amount = 0;
    public int index;
    public SpriteRenderer holdingSprite;
    public Inventory inventory;

    // If inventory spot is selected or not.
    public bool selected
    {
        get
        {
            return IsSelected;
        }
        set
        {
            selecitonImage.enabled = value;
            if (value)
            {
                if (item != null)
                {
                    holdingSprite.sprite = item.sprite;
                    if (item.isTool)
                    {
                        holdingSprite.transform.localScale = new Vector3(1, 1, 0);
                    }
                    else
                    {
                        holdingSprite.transform.localScale = new Vector3(0.5f, 0.5f, 0);
                    }
                }
                else
                {
                    holdingSprite.sprite = null;
                }
            }
            else if (IsSelected)
            {
                holdingSprite.sprite = null;
            }
            IsSelected = value;
        }
    }
    private bool IsSelected = false;
    [SerializeField] private Text text;
    [SerializeField] private Image image;
    [SerializeField] private Image selecitonImage;

    // Awake is called when script instance is loaded.
    private void Awake()
    {
        image.enabled = false;
    }

    // AddItem adds one item of type specified by data.
    public void AddItem(ItemData data)
    {
        if(item != null && data != item)
        {
            Debug.LogError("ading incorect item sort to inventory slot");
        }
        item = data;
        amount++;
        image.sprite = item.sprite;
        if(amount > 1)
        {
            text.text = amount.ToString();
        }
        image.enabled = true;
        if (selected)
        {
            selected = true;
        }
    }

    // Removes one item from inventory sport.
    public void RemoveItem()
    {
        amount--;
        if (amount < 2)
        {
            text.text = "";
            if (amount <= 0)
            {
                image.enabled = false;
                if (selected)
                {
                    inventory.TryFindSameItem(item);
                    holdingSprite.sprite = null;
                }
                item = null;
                amount = 0;
            }
        }
        else
        {
            text.text = amount.ToString();
        }
    }

    // Sets bouth the type of item and the amount of it.
    public void Set(ItemData item, int amount)
    {
        this.item = item;
        this.amount = amount;
        if (amount > 1)
        {
            text.text = amount.ToString();
        }
        else
        {
            text.text = "";
        }
        if(item != null)
        {
            image.sprite = item.sprite;
            image.enabled = true;
        }
        else
        {
            image.enabled = false;
        }
        if (selected && item != null)
        {
            selected = true;
        }
    }

    // CLicked is called when this inventory slot is clicked.
    public void Clicked()
    {
        inventory.selectedInventorySpot = this;
    }
}
