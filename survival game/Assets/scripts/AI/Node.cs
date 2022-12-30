
public class Node : IHeapItem<Node>
{
    public int G;
    public int H;
    //public float speed;
    //public float DPS;
    private float speed;
    private float DPS;
    public int xPos;
    public int yPos;
    public Node parent;
    private int heapIndex;
    public int travelCost;
    public int diagonalExtra;
    public int travelDamage;
    public float health;
    public int F
    {
        get
        {
            return G + H;
        }
    }
    public Node(int x, int y, float Speed, float dps)
    {
        xPos = x;
        yPos = y;
        speed = Speed;
        DPS = dps;
        travelCost = (int)(10 / Speed);
        diagonalExtra = (int)(4 / Speed);
        //diagonalExtra = 4;
        travelDamage = (int)(dps / Speed);
    }
    public void addToSpeed(float value)
    {
        speed *= value;
        travelCost = (int)(10 / speed);
        diagonalExtra = (int)(4 / speed);
        travelDamage = (int)(DPS / speed);
    }
    public void addToDPS(float value)
    {
        DPS += value;
        travelDamage = (int)(DPS / speed);
    }
    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }
    public int CompareTo(Node nodeToCompare)
    {
        int compare = F.CompareTo(nodeToCompare.F);
        if (compare == 0)
        {
            compare = H.CompareTo(nodeToCompare.H);
        }
        return -compare;
    }
}
