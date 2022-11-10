using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        Globals.destructibleObjects = new List<DestructibleObject>();
        Globals.characters = new List<Character>();
    }
    private void Start()
    {
        WorldGeneration.worldBounds = new Vector2(16, 16);
        WorldGeneration.instance.startGame();
    }
}
