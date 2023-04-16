using UnityEngine;

//Item handler convert item indexes to item types and types to indexes.
public class ItemHandler : MonoBehaviour
{
    [SerializeField] private ItemData[] items;
    private static ItemData[] itemsInOrder;

    // Awake is called when script instance is loaded.
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

    // Converts itemIndex to ItemData
    public static ItemData IndexToItem(int index)
    {
        if(index < 0)
        {
            return null;
        }
        else if(index >= itemsInOrder.Length)
        {
            Debug.LogError("item index: " + index.ToString() + " to high");
            return null;
        }
        return itemsInOrder[index];
    }
    
    // Converts itemData to a itemIndex.
    public static int ItemTopIndex(ItemData item)
    {
        if(item == null)
        {
            return -1;
        }
        return item.ItemIndex;
    }
}
