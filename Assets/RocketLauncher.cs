using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : MonoBehaviour
{
    public Transform player;

    public Vector3 mousePos;
    public float angle;
    public Transform firePoint;

    public GameObject bulletPrefab;
    public float bulletForce;


    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerMovement>().transform;
    }

    private void Update()
    {
        transform.position = player.transform.position;

        if (Input.GetKeyDown(KeyCode.E))
        {
            Shoot();
        }
    }

    private void FixedUpdate()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDir = mousePos - transform.position;
        angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 180f;
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, angle);
        
    }

    void Shoot()
    {
        GameObject RocketPre = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = RocketPre.GetComponent<Rigidbody2D>();
        Rocket rocket = RocketPre.GetComponent<Rocket>();
        rocket.startPoint = player.position;
        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
    }
}
