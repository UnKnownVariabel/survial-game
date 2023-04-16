using System;
using UnityEngine;

// Tutorialtask contains the tasks text and requirements.
[Serializable]
public struct TutorialTask
{
    public string name;
    [TextArea] public string text;
    public float minTime, maxTime;
    public ItemData[] requiredItems;
    public int[] requiredBuildings;
    public int mobQuota;
    public float zombiesToSpawn;
    public float wolfToSpawn;
}