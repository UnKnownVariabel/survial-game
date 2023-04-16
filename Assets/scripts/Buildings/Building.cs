
// Building is the class attached to all buildings and it inherits from the class StaticObject.
public class Building : StaticObject
{
    public PlacebleItemData itemData;

    // Start is called before the first frame update.
    protected override void Start()
    {
        base.Start();
        (int x, int y) pos = chunk.TilePos(transform.position);
        chunk.buildings[pos] = this;
        WorldGeneration.instance.actions.Add(new Action(2));
    }

    // Die is called when the destructibleObject runs out of health.
    protected override void Die()
    {
        (int x, int y) pos = chunk.TilePos(transform.position);
        chunk.buildings.Remove(pos);

        // If the item has special removal operations they will be completed in Remove item.
        // One example is the wall being removed from the tilemap.
        itemData.RemoveItem(transform.position);
        WorldGeneration.instance.actions.Add(new Action(2));
        base.Die();
    }
}
