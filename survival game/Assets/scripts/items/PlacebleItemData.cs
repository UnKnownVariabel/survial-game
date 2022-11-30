using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacebleItemData : ItemData
{
    public Sprite placedSprite;
    public float health;
    public override void placeItem(int x, int y)
    {

    }
    public override dynamic ReturnFullClass()
    {
        return this;
    }
}
