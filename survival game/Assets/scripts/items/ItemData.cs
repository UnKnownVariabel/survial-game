using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new item", menuName = "item")]
public class ItemData : ScriptableObject
{
    public Sprite sprite;
    public byte ItemIndex;
    public byte stackSize;
}
