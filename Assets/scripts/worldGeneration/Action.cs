
// Action is store of information about a function to be called in another frame.
// There are three types of actions 0. draw a chunk, 1. remove a chunk and 2. draw map.
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
