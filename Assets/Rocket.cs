using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{

    public Vector3 startPoint;


    public GameObject ExplosionPrefab;



    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject explosion = Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
        Explosion explosions = explosion.GetComponent<Explosion>();
        explosions.StartPoint = startPoint;
        Destroy(gameObject);
    }
}
