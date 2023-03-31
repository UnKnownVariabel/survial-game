using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHandler : MonoBehaviour
{
    [SerializeField] private ItemData[] items;
    private static Dictionary<int, ItemData> itemIndexPairs;

    private void Awake()
    {
        itemIndexPairs = new Dictionary<int, ItemData>();
        for(int i = 0; i < items.Length; i++)
        {
            itemIndexPairs.Add(items[i].ItemIndex, items[i]);
        }
    }

    public static ItemData IndexToItem(int index)
    {
        if(index == -1)
        {
            return null;
        }
        return itemIndexPairs[index];
    }
    
    public static int ItemTopIndex(ItemData item)
    {
        foreach (KeyValuePair<int, ItemData> pair in itemIndexPairs)
        {
            if(pair.Value == item)
            {
                return pair.Key;
            }
        }
        Debug.Log("item not found");
        Debug.Log(item);
        return -1;
    }
}
