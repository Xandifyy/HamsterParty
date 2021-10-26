using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        Screen.fullScreen = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetFullscreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }

    void SetVolume(float vol)
    {
        PlayerPrefs.SetFloat("Volume", vol);
    }

    public void Play()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void Reset()
    {
        PlayerPrefs.SetFloat("PlayerX",0);
        PlayerPrefs.SetFloat("PlayerY",0);
        SceneManager.LoadScene("SampleScene");
    }

    public void Quit()
    {
        Application.Quit();
    }

}
