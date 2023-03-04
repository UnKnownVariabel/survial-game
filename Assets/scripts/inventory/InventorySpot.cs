using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySpot : MonoBehaviour
{
    public ItemData item;
    public int amount = 0;
    public int index;
    public SpriteRenderer holdingSprite;
    public Inventory inventory;
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
                    try
                    {
                        ToolData toolData = (ToolData)item;
                        holdingSprite.transform.localScale = new Vector3 (1, 1, 0);
                    }
                    catch
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

    private void Start()
    {
        image.enabled = false;
    }

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
            holdingSprite.sprite = item.sprite;
            try
            {
                ToolData toolData = (ToolData)item;
                holdingSprite.transform.localScale = new Vector3(1, 1, 0);
            }
            catch
            {
                holdingSprite.transform.localScale = new Vector3(0.5f, 0.5f, 0);
            }
        }
    }
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
                    inventory.tryFindSameItem(item);
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
    public void clicked()
    {
        inventory.selectedInventorySpot = this;
    }
}
