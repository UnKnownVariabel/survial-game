using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new item", menuName = "item")]
public class ItemData : ScriptableObject
{
    public Sprite sprite;
    public byte ItemIndex;
    public byte stackSize;
    public bool isPlaceble = false;
    public bool isTool = false;
    public virtual void placeItem(int x, int y)
    {
        Debug.LogError("cant place non placeble item");
    }
    public virtual dynamic ReturnFullClass()
    {
        return this;
    }
}
