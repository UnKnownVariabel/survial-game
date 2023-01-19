using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new tool", menuName = "tool")]
public class ToolData : ItemData
{
    public float damage, swingTime, knockback;
    public Vector2 size, offset;
}
