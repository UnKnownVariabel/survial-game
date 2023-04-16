
// Ingredient contains an item and the amount of that item needed.
public class Ingredient
{
    // Constructer
    public Ingredient(ItemData data)
    {
        item = data;
        amount = 1;
    }
    public ItemData item;
    public int amount;
}