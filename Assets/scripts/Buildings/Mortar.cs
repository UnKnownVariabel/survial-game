using UnityEngine;

// Mortar is the class attached to all mortars and it inherits from the class Tower.
public class Mortar : Tower
{
    [SerializeField] private Bomb bombPref;

    // SpawnProjectile is called from the mortors shoot animation and spawns a bomb and sets its direction and distance.
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
