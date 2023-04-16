using System.Collections.Generic;
using UnityEngine;

public class CraftingButton : MonoBehaviour
{
    [SerializeField] private CraftingRecipe recipe;
    [SerializeField] private Crafting crafting;
    [SerializeField] private RectTransform sheet;
    [SerializeField] private Transform layoutGroup;
    [SerializeField] private IngredientDisplay ingredientDisplay;

    // Start is called before the first frame update.
    private void Start()
    {
        List<Ingredient> ingredients = crafting.GetIngridients(recipe);
        sheet.sizeDelta = new Vector2 (100, ingredients.Count * 28 + 8);
        sheet.anchoredPosition = new Vector2 (0, -((ingredients.Count * 28 + 8) / 2 + 60));
        foreach(Ingredient ingredient in ingredients)
        {
            IngredientDisplay display = Instantiate(ingredientDisplay, new Vector3(0, 0, 0), Quaternion.identity, layoutGroup);
            display.text.text = ingredient.amount.ToString() + "X";
            display.sprite.sprite = ingredient.item.sprite;
        }
    }

    // Trys to craft the recipe and is called by pressing the button.
    public void Craft()
    {
        crafting.TryCraft(recipe);
    }

    // Shows ingredients needed to craft the item.
    public void ShowRecepie()
    {
        sheet.gameObject.SetActive(true);

    }
    // Hides the ingredients needen to creaft the item.
    public void HideRecepie()
    {
       sheet.gameObject.SetActive(false);
    }
}
