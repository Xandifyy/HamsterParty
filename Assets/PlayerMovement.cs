using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float timeHeld;
    public float accelerationRate;
    public float deaccelerationRate;
    public float speed;

    public AudioSource ballsfx;

    public bool OnSlope = false;

    public float wallMomentum = 2;

    [Range(-100f,100f)]
    public float moreDire;

    public void Update()
    {

    }

    private void Start()
    {
        ballsfx.volume = PlayerPrefs.GetFloat("Volume", 0.5f);
    }

    private void FixedUpdate()
    {
        if (!OnSlope)
        {
            rb.velocity = new Vector2(moreDire, rb.velocity.y);


            if (moreDire > 15)
            {
                return;
            }
            else if (moreDire < -15)
            {
                return;
            }

            moreDire += Input.GetAxisRaw("Horizontal") * accelerationRate * Time.fixedDeltaTime;

            if (Input.GetAxisRaw("Horizontal") == 0)
            {


                if (moreDire > 0)
                {
                    moreDire -= deaccelerationRate * accelerationRate * Time.fixedDeltaTime;
                }
                else
                {
                    moreDire += deaccelerationRate * accelerationRate * Time.fixedDeltaTime;

                }


            }


        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
   

        if (collision.gameObject.tag.Equals("Wall"))
        {
            ballsfx.Play();
            FindObjectOfType<CameraShake>().Noise(.5f * (moreDire / 4), 0.5f * (moreDire / 8));
            moreDire = -moreDire / wallMomentum;
        }

        if (collision.gameObject.tag.Equals("SafeFloor"))
        {

            Vector3 dir = (collision.gameObject.transform.position - gameObject.transform.position).normalized;

            if ((this.transform.position.x - collision.collider.transform.position.x) < 0 && dir.y < 0)
            {
                moreDire = -moreDire / wallMomentum / 2;
                print("hit left");
            }
            else if ((this.transform.position.x - collision.collider.transform.position.x) > 0 && dir.y < 0)
            {
                moreDire = -moreDire / wallMomentum / 2;
                print("hit right");
            }
        }
    }

  
}

