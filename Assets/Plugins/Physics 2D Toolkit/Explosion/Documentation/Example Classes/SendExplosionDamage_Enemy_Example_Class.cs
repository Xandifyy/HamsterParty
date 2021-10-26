using UnityEngine;

public class SendExplosionDamage_Enemy_Example_Class : MonoBehaviour {

    public float health = 100f;

    public void TakeExplosionDamage(float damage)
    {
        health -= damage;
    }
}
