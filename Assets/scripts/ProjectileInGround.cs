using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileInGround : MonoBehaviour
{
    public void Vanish()
    {
        Destroy(gameObject);
    }
}
