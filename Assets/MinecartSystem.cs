using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinecartSystem : MonoBehaviour
{

    public Transform pos1;
    public Transform pos2;

    public Transform des;
    public SpriteRenderer sprite;
    public Transform minecart;

    public float speed = 10f;
    // Start is called before the first frame update
    void Start()
    {
        des = pos2;
        minecart.position = pos1.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        minecart.position = Vector3.Lerp(pos1.position, pos2.position, Mathf.PingPong(Time.time * speed, 1.0f));
        if(minecart.position == pos1.position)
        {
            sprite.flipX = true;
        }
        if (minecart.position == pos2.position)
        {
            sprite.flipX = false;
        }
    }
}
