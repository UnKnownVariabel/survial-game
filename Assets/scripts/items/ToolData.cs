using UnityEngine;

// ToolData contains information about a tool.
[CreateAssetMenu(fileName = "new tool", menuName = "tool")]
public class ToolData : ItemData
{
    public float damage, swingTime, knockback;
    public Multipliers multipliers;
    public LayerMask layerMask;
    public Vector2 size, offset;
}
