using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySpot : MonoBehaviour
{
    public ItemData item;
    public int amount = 0;
    [SerializeField] private Text text;
    [SerializeField] private Image image;

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
}
