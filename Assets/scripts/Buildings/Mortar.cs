using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Mortar : Tower
{
    [SerializeField] private Bomb bombPref;
    public override void SpawnProjectile()
    {
        Bomb bomb = Instantiate(bombPref, spawnPoint.position, Quaternion.identity);
        Vector3 prediction = target.transform.position + (Vector3)target.rb.velocity * bombPref.loiteringTime;
        bomb.direction = (prediction - spawnPoint.position).normalized;
        bomb.distance = Vector2.Distance(prediction, spawnPoint.position);
    }
}
