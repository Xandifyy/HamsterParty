using UnityEngine;
using ExplosionForce2D;

public class AddExplosionForce2D_Example_Class : MonoBehaviour {

    public float force = 25f;
    public float radius = 4f;
    public bool modifyForceByDistance = true;
    public Vector3 offset = default(Vector3);
    public ForceMode2D forceMode = ForceMode2D.Impulse;

    private Collider2D[] colliders = new Collider2D[] { };

    void Explode()
    {
        colliders = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (Collider2D hit in colliders)
        {
            Rigidbody2D Rb = hit.GetComponent<Rigidbody2D>();
            if (Rb)
                Rb.AddExplosionForce2D(force, transform.position + offset, radius, modifyForceByDistance, forceMode);
        }
    }

    void Start()
    {
        Explode();
    }
}
