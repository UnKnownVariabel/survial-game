using UnityEngine;

// Data for a item which can be placed.
[CreateAssetMenu(fileName = "new plable item", menuName = "placable item")]
public class PlacebleItemData : ItemData
{
    public Building prefab;

    // Is called when item is to be placed.
    public virtual Building placeItem(Vector3 position)
    {
        Vector3 realPos = Globals.wallTilemap.WorldToCell(position) + new Vector3(0.5f, 0.5f);
        Building building = Instantiate(prefab, realPos, Quaternion.identity);
        return building;
    }

    // Is called when item is to be destroyd.
    public virtual void RemoveItem(Vector2 position)
    {

    }
}
