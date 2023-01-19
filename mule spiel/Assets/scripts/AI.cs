using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    private struct AIposition
    {
        public int state;
        public (int horizontal, int vertical) mills;
    }

    public readonly int[,] mills = new int[16, 3]
    {
        { 1, 2, 3 },
        { 4, 5, 6 },
        { 7, 8, 9 },
        { 10, 11, 12 },
        { 13, 14, 15 },
        { 16, 17, 18 },
        { 19, 20, 21 },
        { 22, 23, 24 },
        { 1, 10, 22 },
        { 4, 11, 19 },
        { 7, 12, 16 },
        { 2, 5, 8 },
        { 17, 20, 23 },
        { 9, 13, 18 },
        { 6, 14, 21 },
        { 3, 15, 24 }
    };

    private AIposition[] positions = new AIposition[24];

    private void Awake()
    {
        for(int x = 0; x < 8; x++)
        {
            for(int y = 0; y < mills.GetLength(1); y++)
            {
                positions[mills[x, y]].mills.horizontal = x;
            }
        }
        for (int x = 8; x < 16; x++)
        {
            for (int y = 0; y < mills.GetLength(1); y++)
            {
                positions[mills[x, y]].mills.vertical = x;
            }
        }
    }

    private bool isInMill(int i)
    {
        return positions[i].state != 0 && ((positions[i].state == mills[positions[i].mills.horizontal, 0] && positions[i].state == mills[positions[i].mills.horizontal, 1] && positions[i].state == mills[positions[i].mills.horizontal, 2]) || (positions[i].state == mills[positions[i].mills.vertical, 0] && positions[i].state == mills[positions[i].mills.vertical, 1] && positions[i].state == mills[positions[i].mills.vertical, 2]));
    }
}
