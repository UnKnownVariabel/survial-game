using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new tool", menuName = "tool")]
public class ToolData : ItemData
{
    public float damage, swingTime;
    public override dynamic ReturnFullClass()
    {
        return this;
    }
}
