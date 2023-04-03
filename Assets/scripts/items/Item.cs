using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData data;

    [SerializeField] private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer.sprite = data.sprite;
        Globals.chunks[(Mathf.RoundToInt(transform.position.x / 16), Mathf.RoundToInt(transform.position.y / 16))].AddItem(this);
    }
}
