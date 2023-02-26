using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public DestructibleObject target;
    public float DPS;
    private void Update()
    {
        target.TakeDamage(DPS * Time.deltaTime);
    }
}
