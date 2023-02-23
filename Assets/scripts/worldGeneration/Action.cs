public struct Action
{
    public byte type;
    public int x, y;
    public Action(byte Type, int X, int Y)
    {
        type = Type;
        x = X;
        y = Y;
    }
}
