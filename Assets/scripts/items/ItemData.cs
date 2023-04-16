using UnityEngine;

// ItemData is the class all other item inherit from and contains the information they all need.
[CreateAssetMenu(fileName = "new item", menuName = "item")]
public class ItemData : ScriptableObject
{
    public Sprite sprite;
    public byte ItemIndex;
    public byte stackSize;
    public bool isPlaceble = false;
    public bool isTool = false;
    public bool isEdible = false;
}
