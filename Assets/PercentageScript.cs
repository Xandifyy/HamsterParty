using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PercentageScript : MonoBehaviour
{


    public float percentage;
    public Transform playerTransform;
    public Transform topTransform;
    public TextMeshProUGUI PercentText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        percentage = Mathf.RoundToInt((playerTransform.position.y / topTransform.position.y) * 100);
        PercentText.text = percentage + "%";
    }
}
