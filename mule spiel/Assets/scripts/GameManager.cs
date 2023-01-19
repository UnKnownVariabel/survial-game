using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Position[] positions = new Position[24];
    private bool isWhiteMove = true;
    public int mode;
    private Position selectedPosition
    {
        get
        {
            return _selectedPosition;
        }
        set
        {
            if(_selectedPosition != null)
            {
                _selectedPosition.isSelected = false;
            }
            _selectedPosition = value;
            if (_selectedPosition != null)
            {
                _selectedPosition.isSelected = true;
            }
        }
    }
    private Position _selectedPosition;
    private int move = 0;
    private (int white, int black) count;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Presed();
        }
    }
    private void Presed()
    {
        Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(mode == 0 && move >= 18)
        {
            mode = 2;
        }
        switch (mode)
        {
            case 0:
                Place();
                break;
            case 1:
                Remove();
                break;
            case 2:
                Move();
                break;
        }
        
        void Place()
        {
            for (int i = 0; i < positions.Length; i++)
            {
                if (positions[i].Check(position))
                {
                    if (positions[i].state == 0)
                    {
                        if (isWhiteMove)
                        {
                            positions[i].state = 1;
                        }
                        else
                        {
                            positions[i].state = 2;
                        }
                        if (positions[i].isInMill())
                        {
                            Debug.Log("position is in mill");
                            mode = 1;
                        }
                        else
                        {
                            isWhiteMove = !isWhiteMove;
                        }
                        UpdateCount();

                        move++;
                    }
                    break;
                }
            }
        }

        void Remove()
        {
            for (int i = 0; i < positions.Length; i++)
            {
                if((positions[i].state == 1 && !isWhiteMove) || (positions[i].state == 2 && isWhiteMove))
                {
                    if (positions[i].Check(position) && !positions[i].isInMill())
                    {
                        positions[i].state = 0;
                        mode = 0;
                        isWhiteMove = !isWhiteMove;
                        UpdateCount();
                        break;
                    }
                }
            }
        }

        void Move()
        {
            for (int i = 0; i < positions.Length; i++)
            {
                if(selectedPosition != null && positions[i].state == 0)
                {
                    if (positions[i].Check(position))
                    {
                        if ((isWhiteMove && count.white < 4) || (!isWhiteMove && count.black < 4))
                        {
                            positions[i].state = selectedPosition.state;
                            selectedPosition.state = 0;
                            if (positions[i].isInMill())
                            {
                                mode = 1;
                            }
                            else
                            {
                                isWhiteMove = !isWhiteMove;
                            }
                            selectedPosition = null;
                        }
                        else
                        {
                            for (int I = 0; I < positions[i].horizontalNeighbours.Length; I++)
                            {
                                if (positions[i].horizontalNeighbours[I] == selectedPosition)
                                {
                                    positions[i].state = selectedPosition.state;
                                    selectedPosition.state = 0;
                                    if (positions[i].isInMill())
                                    {
                                        mode = 1;
                                    }
                                    else
                                    {
                                        isWhiteMove = !isWhiteMove;
                                    }
                                    selectedPosition = null;
                                }
                            }
                            for (int I = 0; I < positions[i].verticalNeighbours.Length; I++)
                            {
                                if (positions[i].verticalNeighbours[I] == selectedPosition)
                                {
                                    positions[i].state = selectedPosition.state;
                                    selectedPosition.state = 0;
                                    if (positions[i].isInMill())
                                    {
                                        mode = 1;
                                    }
                                    else
                                    {
                                        isWhiteMove = !isWhiteMove;
                                    }
                                    selectedPosition = null;
                                }
                            }
                        }
                        
                    }
                }
                else if ((positions[i].state == 1 && isWhiteMove) || (positions[i].state == 2 && !isWhiteMove))
                {
                    if (positions[i].Check(position))
                    {
                        selectedPosition = positions[i];
                    }
                }
            }
        }
    }
    private void UpdateCount()
    {
        count.white = 0;
        count.black = 0;
        for (int i = 0; i < positions.Length; i++)
        {
            if(positions[i].state == 1)
            {
                count.white++;
            }
            else if (positions[i].state == 2)
            {
                count.black++;
            }
        }
        Debug.Log(count);
        if(move >= 18)
        {
            if (count.white < 3)
            {
                Debug.Log("black won");
            }
            else if (count.black < 3)
            {
                Debug.Log("white won");
            }
        }
    }
}
