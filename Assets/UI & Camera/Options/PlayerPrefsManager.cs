using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour
{
    const string MUSIC_VOLUME_KEY = "music_volume";
    const string MASTER_VOLUME_KEY = "master_volume";
    const string PLAYER_NICKNAME_KEY = "player_nickname";
    const string WEATHER_EFFECTS_KEY = "weather_effects";
    const string RAIN_KEY = "rain";
    const string LAST_IP_KEY = "last_ip";

    public static void SetMusicVolume(float volume)
    {
        if (volume >= 0f && volume <= 1f)
        {
            PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, volume);
        }
        else
        {
            Debug.LogError("Music volume out of range!");
        }
    }

    public static float GetMusicVolume()
    {
        return PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY);
    }

    public static void SetMasterVolume(float volume)
    {
        if (volume >= 0f && volume <= 1f)
        {
            PlayerPrefs.SetFloat(MASTER_VOLUME_KEY, volume);
        }
        else
        {
            Debug.LogError("Master volume out of range!");
        }
    }

    public static float GetMasterVolume()
    {
        return PlayerPrefs.GetFloat(MASTER_VOLUME_KEY);
    }

    public static void SetPlayerNickname(string nickname)
    {
        PlayerPrefs.SetString(PLAYER_NICKNAME_KEY, nickname);
    }

    public static string GetPlayerNickname()
    {
        return PlayerPrefs.GetString(PLAYER_NICKNAME_KEY);
    }

    public static void SetWeatherEffects(int weather)
    {
        PlayerPrefs.SetInt(WEATHER_EFFECTS_KEY, weather);
    }

    public static int GetWeatherEffects()
    {
        return PlayerPrefs.GetInt(WEATHER_EFFECTS_KEY);
    }

    public static void SetRain(int rain)
    {
        PlayerPrefs.SetInt(RAIN_KEY, rain);
    }

    public static int GetRain()
    {
        return PlayerPrefs.GetInt(RAIN_KEY);
    }

    public static void SetLastIP(string ipadress)
    {
        PlayerPrefs.SetString(LAST_IP_KEY, ipadress);
    }

    public static string GetLastIP()
    {
        return PlayerPrefs.GetString(LAST_IP_KEY);
    }
}
