using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public float pickUpDistance = 0.5f;
    [SerializeField] private InventorySpot[] inventory = new InventorySpot[10];
    private Vector2 direction;

    private void Awake()
    {
        /*for(int i = 0; i < inventory.Length; i++)
        {
            inventory[i] = new InventorySpot();
        }*/
    }

    void Update()
    {
        // movement
        direction = Vector2.zero;
        if (Input.GetKey(KeyCode.W))
        {
            direction.y += 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction.y -= 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction.x += 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction.x -= 1;
        }
        Move(direction);

        if (Input.GetKey(KeyCode.Q))
        {
            PickUpItems();
        }
    }
    void PickUpItems()
    {
        foreach(Item item in Globals.currentChunk.items)
        {
            if(Vector2.Distance(item.transform.position, transform.position) < pickUpDistance)
            {
                for(int i = 0; i < inventory.Length; i++)
                {
                    if(inventory[i].item == null || (inventory[i].item == item.data && inventory[i].amount < item.data.stackSize))
                    {
                        inventory[i].AddItem(item.data);
                        break;
                    }
                    
                }
                Globals.currentChunk.items.Remove(item);
                Destroy(item.gameObject);
                break;
            }
        }
    }
}
