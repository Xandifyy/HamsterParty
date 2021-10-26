using System.Collections;
using System.Collections.Generic;
using System.Windows;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public float time;
    bool timerIsCounting;
    public GameMaster gameMaster;
    string timeText;
    public TextMeshProUGUI timerText;
    // Start is called before the first frame update
    void Start()
    {
        timerIsCounting = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerIsCounting)
        {
            time += Time.smoothDeltaTime;
            Displaytime(time);
        }
    }
    void Displaytime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText = string.Format("{0:00}:{1:00}", minutes, seconds);
        timerText.text = timeText;
        PlayerPrefs.SetString("Time", timeText);
    }
}
