
// StaticObject is the class all non moving objects inherit from like buildings.
// It is also directly attached to trees, stones and stumps.

public class StaticObject : DestructibleObject
{
    public byte objectIndex;

    // Start is called before the first frame update.
    protected override void Start()
    {
        base.Start();
        chunk = Globals.GetChunk(transform.position);
        (int x, int y) pos = chunk.TilePos(transform.position);
        int rest = chunk.tiles[pos.x, pos.y] % 16;
        chunk.tiles[pos.x, pos.y] = (byte)(objectIndex * 16 + rest);
        chunk.staticObjects[pos] = this;
    }

    // Die is called when the destructibleObject runs out of health.
    protected override void Die()
    {
        (int x, int y) pos = chunk.TilePos(transform.position);
        int rest = chunk.tiles[pos.x, pos.y] % 16;
        chunk.tiles[pos.x, pos.y] = (byte)rest;
        chunk.staticObjects.Remove(pos);
        base.Die();
    }
}
