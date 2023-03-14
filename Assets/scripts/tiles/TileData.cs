using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
[CreateAssetMenu(fileName = "new tile data", menuName = "tile date")]
public class TileData : ScriptableObject
{
    public TileBase[] tiles;
    public float DPS, speed, plantSurvivability;
}
