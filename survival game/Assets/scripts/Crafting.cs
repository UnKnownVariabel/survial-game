using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient
{
    public Ingredient(ItemData data)
    {
        item = data;
        amount = 1;
    }
    public ItemData item;
    public int amount;
}

public class Crafting : MonoBehaviour
{
    public Inventory inventory;
    public Player player;
    public Item itemPrefab;
    public void TryCraft(CraftingRecipe recipe)
    {
        List<Ingredient> ingredients = new List<Ingredient>();
        for(int i = 0; i < recipe.input.Length; i++)
        {
            foreach(Ingredient ingredient in ingredients)
            {
                if(ingredient.item == recipe.input[i])
                {
                    ingredient.amount += 1;
                    goto OuterLoop;
                }
            }
            ingredients.Add(new Ingredient(recipe.input[i]));
            OuterLoop:
                continue;
        }
        foreach(Ingredient ingredient in ingredients)
        {
            if (!inventory.checkForIngredient(ingredient))
            {
                return;
            }
        }
        foreach (Ingredient ingredient in ingredients)
        {
            inventory.RemoveIngredient(ingredient);
        }
        for (int i = 0; i < recipe.output.Length; i++)
        {
            if (!inventory.pickUpItem(recipe.output[i]))
            {
                Item item = Instantiate(itemPrefab, player.transform.position, Quaternion.identity);
                item.data = recipe.output[i];
            }
        }
    }
}
