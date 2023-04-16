using UnityEngine;

// CraftingRecipe contains the items needed to craft a recipe and the items that are created.
[CreateAssetMenu(fileName = "new crafting recipe", menuName = "creafting recipe")]
public class CraftingRecipe : ScriptableObject
{
    public ItemData[] input;
    public ItemData[] output;
}
