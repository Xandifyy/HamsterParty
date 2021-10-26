using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHoverEvent : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void FlipButton(RectTransform button)
    {
        Debug.Log(button.localRotation.z * Mathf.Rad2Deg);
        button.localRotation = Quaternion.Euler(button.localRotation.x, button.localRotation.y, button.localRotation.z * Mathf.Rad2Deg * 2 * -1);
    }

}
