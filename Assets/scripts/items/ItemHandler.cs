using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHandler : MonoBehaviour
{
    [SerializeField] private ItemData[] items;
    private static ItemData[] itemsInOrder;

    private void Awake()
    {
        itemsInOrder = new ItemData[items.Length];
        for(int i = 0; i < items.Length; i++)
        {
            if (itemsInOrder[items[i].ItemIndex] != null)
            {
                Debug.LogError("two items of same index in item handler");
            }
            itemsInOrder[items[i].ItemIndex] = items[i];
        }
    }

    public static ItemData IndexToItem(int index)
    {
        if(index < 0)
        {
            Debug.LogError("item index: " + index.ToString() + " to low");
            return null;
        }
        else if(index >= itemsInOrder.Length)
        {
            Debug.LogError("item index: " + index.ToString() + " to high");
            return null;
        }
        return itemsInOrder[index];
    }
    
    public static int ItemTopIndex(ItemData item)
    {
        if(item == null)
        {
            Debug.LogError("item null");
            return -1;
        }
        return item.ItemIndex;
    }
}
