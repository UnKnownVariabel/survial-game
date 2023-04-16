
// Multipliers contains multipliers for tool against the diffrent destructible object types.
[System.Serializable]
public struct Multipliers
{
    public float mob, wood, stone;

    // Constructor.
    public Multipliers(float Mob, float Wood, float Stone)
    {
        mob = Mob; 
        wood = Wood; 
        stone = Stone;
    }
    public static readonly Multipliers One = new Multipliers(1, 1, 1);
}
