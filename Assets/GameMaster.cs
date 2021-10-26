using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{

    public GameObject Player;

    public AudioSource music;

    // Start is called before the first frame update
    void Start()
    {
        music.volume = PlayerPrefs.GetFloat("Volume", 0.5f);
        Player.transform.position = new Vector3(PlayerPrefs.GetFloat("PlayerX", -12), PlayerPrefs.GetFloat("PlayerY"), -6);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetFloat("PlayerX", Player.transform.position.x);
        PlayerPrefs.SetFloat("PlayerY", Player.transform.position.y);

    }
}
