
// Spikes is the class attached to all spikes and it inherits from the class Building.
public class Spikes : Building
{
    public float DPS;
    public float speed;
    private float baseSpeed;

    // Start is called before the first frame update.
    protected override void Start()
    {
        base.Start();
        (int x, int y) pos = chunk.TilePos(transform.position);
        chunk.DPS[pos.x, pos.y] = DPS;
        baseSpeed = chunk.speed[pos.x, pos.y];
        chunk.speed[pos.x, pos.y] = 1 / (1 / baseSpeed + 1 / speed);
    }

    // Die is called when the destructibleObject runs out of health.
    protected override void Die()
    {
        (int x, int y) pos = chunk.TilePos(transform.position);
        chunk.DPS[pos.x, pos.y] = 0;
        chunk.speed[pos.x, pos.y] = baseSpeed;
        base.Die();
    }

}
