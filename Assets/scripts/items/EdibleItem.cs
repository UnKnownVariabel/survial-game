using UnityEngine;

// Item which is edible.
[CreateAssetMenu(fileName = "new edible item", menuName = "edible item")]
public class EdibleItem : ItemData
{
    public float health;
}
