using UnityEngine;

// Fire is atached to the fire prefab which ataches to a destructible object and slowly destroys it.
public class Fire : MonoBehaviour
{
    public DestructibleObject target;
    public float DPS;

    // Update is called once per frame.
    private void Update()
    {
        target.TakeDamage(DPS * Time.deltaTime);
    }
}
