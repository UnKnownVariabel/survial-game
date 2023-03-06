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