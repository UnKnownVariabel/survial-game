using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new crafting recipe", menuName = "creafting recipe")]

public class CraftingRecipe : ScriptableObject
{
    public ItemData[] input;
    public ItemData[] output;
}
