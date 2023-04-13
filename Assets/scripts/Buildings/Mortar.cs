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
        Vector3 prediction;
        if(target != null)
        {
            prediction = target.transform.position + (Vector3)target.rb.velocity * bombPref.loiteringTime;
        }
        else
        {
            prediction = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0).normalized * range;
        }
        bomb.direction = (prediction - spawnPoint.position).normalized;
        bomb.distance = Vector2.Distance(prediction, spawnPoint.position);
    }
}
