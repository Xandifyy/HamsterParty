using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{

    public float dist;
    public float angle;

    public SingleExplosion singleExplosion;

    public Vector3 StartPoint;
    public Vector3 EndPoint;

    public Vector3 vecDist;

    public Rigidbody2D player;

    public void Start()
    {
        player = FindObjectOfType<PlayerMovement>().GetComponent<Rigidbody2D>();
        EndPoint = transform.position;
        FindDistance();
        FindAngle();
        Explode();

        Destroy(gameObject, 0.3f);
    }

    void FindDistance()
    {
        dist = Mathf.Sqrt(Mathf.Pow(EndPoint.x - StartPoint.x, 2) + Mathf.Pow(EndPoint.y - StartPoint.y, 2));
        vecDist = StartPoint - EndPoint;
        vecDist = new Vector3(vecDist.x, vecDist.y / 4, vecDist.z);
    }

    void FindAngle()
    {
        angle = Mathf.Atan2(EndPoint.y - StartPoint.y, EndPoint.x - StartPoint.x) * Mathf.Rad2Deg;
        // angle = Vector2.Angle(StartPoint, EndPoint);


    }

    void Explode()
    {
        if(dist > 7)
        {
            return;
        }

        singleExplosion.Activate();


        //player.AddForce(vecDist * 100 / dist);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
