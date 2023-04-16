
// Nodes are a representation of a tile the Pathfinder uses to create paths.
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

    // Finds F cost wich is the sum G cost and H cost.
    public int F
    {
        get
        {
            return G + H;
        }
    }

    // Constructor.
    public Node(int x, int y, float Speed, float dps)
    {
        xPos = x;
        yPos = y;
        speed = Speed;
        DPS = dps;
        travelCost = (int)(10 / Speed);
        diagonalExtra = (int)(4 / Speed);
        travelDamage = (int)(dps / Speed);
    }

    // Change the traveling speed over node after the node has already been constructed.
    public void AddToSpeed(float value)
    {
        speed *= value;
        travelCost = (int)(10 / speed);
        diagonalExtra = (int)(4 / speed);
        travelDamage = (int)(DPS / speed);
    }

    // Change the DPS over node after the node has already been constructed.
    public void AddToDPS(float value)
    {
        DPS += value;
        travelDamage = (int)(DPS / speed);
    }

    // Index in heap.
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

    // Used to sort nodes in heap.
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
