using UnityEngine;

//Item is the class attached to item prefab which represents an item when it i on the ground.
public class Item : MonoBehaviour
{
    public ItemData data;

    [SerializeField] private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update.
    void Start()
    {
        spriteRenderer.sprite = data.sprite;
    }
}
