using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockRo : MonoBehaviour
{

    Quaternion iniRot;
  
    void Start()
    {
        iniRot = transform.rotation;
    }

    void Update()
    {
        iniRot.y = transform.eulerAngles.y; // keep current rotation about Y
        transform.rotation = iniRot; // restore original rotation with new Y
    }
}
