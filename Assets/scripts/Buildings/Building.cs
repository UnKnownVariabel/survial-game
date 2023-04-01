
public class Building : StaticObject
{
    public PlacebleItemData itemData;

    protected override void Start()
    {
        base.Start();
        (int x, int y) pos = chunk.TilePos(transform.position);
        chunk.buildings[pos] = this;
        WorldGeneration.instance.actions.Add(new Action(2));
    }
    protected override void Die()
    {
        (int x, int y) pos = chunk.TilePos(transform.position);
        chunk.buildings.Remove(pos);
        itemData.RemoveItem(transform.position);
        WorldGeneration.instance.actions.Add(new Action(2));
        base.Die();
    }
}
