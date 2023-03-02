public struct Action
{
    public byte type;
    public int x, y;
    public Action(byte Type, int X = 0, int Y = 0)
    {
        type = Type;
        x = X;
        y = Y;
    }
}
