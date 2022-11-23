using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySpot : MonoBehaviour
{
    public ItemData item;
    public int amount = 0;
    public int index;
    public bool selected
    {
        get
        {
            return IsSelected;
        }
        set
        {
            IsSelected = value;
            selecitonImage.enabled = value;
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
    }
    public void RemoveItem()
    {
        amount--;
        if(amount < 2)
        {
            text.text = "";
            if(amount <= 0)
            {
                image.enabled = false;
                item = null;
            }
        }
        else
        {
            text.text = amount.ToString();
        }
    }
}
