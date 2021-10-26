using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FInalTime : MonoBehaviour
{

    public TextMeshProUGUI timer;

    // Start is called before the first frame update
    void Start()
    {
        timer.text = PlayerPrefs.GetString("Time", "0:00");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
