using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : SimpleMenu<OptionsMenu>
{
    [SerializeField] private GameObject incorrectNamePanel;
    [SerializeField] private InputField nameField;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Toggle rainToggle;
    [SerializeField] private Toggle weatherEffectsToggle;

    private const int MIN_NAME_LENGTH = 3;
    private const int MAX_NAME_LENGTH = 12;

    void Start()
    {
        incorrectNamePanel.SetActive(false);
        RestoreAllProperties();
    }

    public override void OnBackPressed()
    {
        if (IsNameCorrect())
        {
            SaveAllProperties();
            MainMenu.Show();
        }
        else
        {
            incorrectNamePanel.SetActive(true);
        }
    }

    public void OnIncorrectNamePanelPressed()
    {
        incorrectNamePanel.SetActive(false);
        nameField.text = "";
    }

    private bool IsNameCorrect()
    {
        string nickname = nameField.text;

        if(nickname.Length < MIN_NAME_LENGTH || nickname.Length > MAX_NAME_LENGTH)
            return false;

        foreach (char c in nickname)
        {
            // only numbers, uppercase and lowercase letters in nickname
            if (c < 48 || c > 57 && c < 65 || c > 90 && c < 97 || c > 122)
                return false;
        }
        return true;
    }

    public void OnChangeMusicVolumeSlider()
    {
        foreach (AudioSource audioSource in FindObjectsOfType<AudioSource>())
        {
            audioSource.volume = musicSlider.value;
        }
    }

    public void OnChangeMasterVolumeSlider()
    {
        AudioListener.volume = masterSlider.value;
    }

    private void SaveAllProperties()
    {
        PlayerPrefsManager.SetPlayerNickname(nameField.text);
        PlayerPrefsManager.SetMusicVolume(musicSlider.value);
        PlayerPrefsManager.SetMasterVolume(masterSlider.value);
        PlayerPrefsManager.SetRain(rainToggle.isOn ? 1 : 0);
        PlayerPrefsManager.SetWeatherEffects(weatherEffectsToggle.isOn ? 1 : 0);
    }

    private void RestoreAllProperties()
    {
        nameField.text = PlayerPrefsManager.GetPlayerNickname();
        musicSlider.value = PlayerPrefsManager.GetMusicVolume();
        masterSlider.value = PlayerPrefsManager.GetMasterVolume();
        rainToggle.isOn = (PlayerPrefsManager.GetRain() == 1);
        weatherEffectsToggle.isOn = (PlayerPrefsManager.GetWeatherEffects() == 1);
    }
}
