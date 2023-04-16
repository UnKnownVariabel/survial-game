using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Crafting handles the crafting of items.
public class Crafting : MonoBehaviour
{
    public Inventory inventory;
    public Player player;
    public Item itemPrefab;
    public int[] resources;
    public CraftingRecipe[] recipes;
    public Text[] texts;

    [SerializeField] private Transform recipeSheet;

    // Chekcks the amount of each crafting recipe the playe could potentially make
    // then writes that above each recipe.
    public void UpdatePotentialValues()
    {
        UpdateResources();
        for(int i = 0; i < recipes.Length; i++)
        {
            int minAmount = int.MaxValue;
            List<Ingredient> ingredients = GetIngridients(recipes[i]);
            foreach(Ingredient ingredient in ingredients)
            {
                int amount = resources[ingredient.item.ItemIndex] / ingredient.amount;
                if (amount < minAmount)
                {
                    minAmount = amount;
                }
            }
            texts[i].text = (minAmount * recipes[i].output.Length).ToString();
        }
    }

    // Checks the amount of each item the player has in his inventory.
    private void UpdateResources()
    {
        resources = new int[resources.Length];
        foreach(InventorySpot inventorySpot in inventory.spots)
        {
            if(inventorySpot.item != null)
            {
                resources[inventorySpot.item.ItemIndex] += inventorySpot.amount;
            }
        }
    }

    // Trys to craft an item with a specified recipie.
    public void TryCraft(CraftingRecipe recipe)
    {
        List<Ingredient> ingredients = GetIngridients(recipe);
        foreach(Ingredient ingredient in ingredients)
        {
            if (!inventory.CheckForIngredient(ingredient))
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
            if (!inventory.PickUpItem(recipe.output[i]))
            {
                Item item = Instantiate(itemPrefab, player.transform.position + new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f)), Quaternion.identity);
                item.data = recipe.output[i];
                Globals.chunks[(Mathf.RoundToInt(item.transform.position.x / 16), Mathf.RoundToInt(item.transform.position.y / 16))].AddItem(item);
            }
        }
        UpdatePotentialValues();
    }

    // Converts Crafting recipe to list of ingredients.
    public List<Ingredient> GetIngridients(CraftingRecipe recipe)
    {
        List<Ingredient> ingredients = new List<Ingredient>();
        for (int i = 0; i < recipe.input.Length; i++)
        {
            foreach (Ingredient ingredient in ingredients)
            {
                if (ingredient.item == recipe.input[i])
                {
                    ingredient.amount += 1;
                    goto OuterLoop;
                }
            }
            ingredients.Add(new Ingredient(recipe.input[i]));
        OuterLoop:
            continue;
        }
        return ingredients;
    }
    
}
