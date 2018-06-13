using UnityEngine;
using DigitalRuby.RainMaker;

public class OptionsHandler : MonoBehaviour
{
    // it is used to set all options in game like it's set in options
    // TODO it can't change volume of objects dynamically instantiated, a better system needed

	void Start ()
    {
        float musicVolume = PlayerPrefsManager.GetMusicVolume();

        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
        foreach(AudioSource audio in audioSources)
        {
            if(audio.spatialBlend == 0) // 2D audio
            {
                audio.volume = audio.volume * musicVolume;
            }
        }

        if (PlayerPrefsManager.GetRain() == 1)
        {
            FindObjectOfType<RainScript>().gameObject.SetActive(false);
        }

        if(PlayerPrefsManager.GetWeatherEffects() == 1)
        {
            FindObjectOfType<RainCameraController>().gameObject.SetActive(false);
        }

        Destroy(gameObject);
    }
}
