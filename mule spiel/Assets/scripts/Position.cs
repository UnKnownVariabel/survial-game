using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Position : MonoBehaviour
{
    public SpriteRenderer sprite;
    public SpriteRenderer selectionSprite;
    public Color white;
    public Color black;
    public Position[] horizontalNeighbours;
    public Position[] verticalNeighbours;
    public int state
    {
        set
        {
            _state = value;
            if(value == 0)
            {
                sprite.color = new Color(0, 0, 0, 0);
            }
            else if(value == 1)
            {
                sprite.color = white;
            }
            else if(value == 2)
            {
                sprite.color = black;
            }
            else
            {
                Debug.Log("this is not a valid state for a position on the board: " + value.ToString());
            }
        }
        get
        {
            return _state;
        }
    }
    private int _state;

    public bool isSelected
    {
        set
        {
            selectionSprite.enabled = value;
        }
    }

    private void Start()
    {
        for(int i = 0; i < horizontalNeighbours.Length; i++)
        {
            bool y = false;
            for(int x = 0; x < horizontalNeighbours[i].horizontalNeighbours.Length; x++)
            {
                if(horizontalNeighbours[i].horizontalNeighbours[x] == this)
                {
                    y = true;
                }
            }
            if (!y)
            {
                Debug.Log(gameObject.name);
            }
        }
        for (int i = 0; i < verticalNeighbours.Length; i++)
        {
            bool y = false;
            for (int x = 0; x < verticalNeighbours[i].verticalNeighbours.Length; x++)
            {
                if (verticalNeighbours[i].verticalNeighbours[x] == this)
                {
                    y = true;
                }
            }
            if (!y)
            {
                Debug.Log(gameObject.name);
            }
        }
    }

    public bool Check(Vector2 pos)
    {
        return Vector2.Distance(pos, transform.position) < 0.5;
    }

    public bool isInMill()
    {
        if(state == 0)
        {
            return false;
        }

        if(horizontalNeighbours.Length > 1)
        {
            if(horizontalNeighbours[0].state == state && horizontalNeighbours[1].state == state)
            {
                return true;
            }
        }
        else if(horizontalNeighbours[0].horizontalNeighbours[0].state == state && horizontalNeighbours[0].horizontalNeighbours[1].state == state && horizontalNeighbours[0].state == state)
        {
            return true;
        }
        if (verticalNeighbours.Length > 1)
        {
            if (verticalNeighbours[0].state == state && verticalNeighbours[1].state == state)
            {
                return true;
            }
        }
        else if (verticalNeighbours[0].verticalNeighbours[0].state == state && verticalNeighbours[0].verticalNeighbours[1].state == state && verticalNeighbours[0].state == state)
        {
            return true;
        }
        return false;

    }
}
