using UnityEngine;

// Class is atached to arrow in ground.
public class ProjectileInGround : MonoBehaviour
{
    // Vanish destroys gameObject.
    public void Vanish()
    {
        Destroy(gameObject);
    }
}
