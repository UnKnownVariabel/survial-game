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
        bomb.direction = (target.transform.position - spawnPoint.position).normalized;
        bomb.distance = Vector2.Distance(target.transform.position, spawnPoint.position);
    }
}
