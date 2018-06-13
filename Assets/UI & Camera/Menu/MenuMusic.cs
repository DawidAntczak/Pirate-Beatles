using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuMusic : MonoBehaviour
{
	void Start ()
	{
	    AudioListener.volume = PlayerPrefsManager.GetMasterVolume();
        GetComponent<AudioSource>().volume = PlayerPrefsManager.GetMusicVolume();
	}
}
