using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRotater : MonoBehaviour
{

    public PlayerMovement playerMovement;
    public float rotateSpeed = 3f; 

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z + playerMovement.moreDire * rotateSpeed * Time.fixedDeltaTime * 100);
    }
}
