using UnityEngine;
using UnityEngine.UI;

public class MainMenu : SimpleMenu<MainMenu>
{
    [SerializeField] private InputField ipInputField;

    private void Start()
    {
        ipInputField.text = PlayerPrefsManager.GetLastIP();
    }

    public void OnHostGamePressed()
    {
        MyNetworkManager.Instance.MyStartHost();
    }

    public void OnJoinGamePressed()
    {
        MyNetworkManager.Instance.MyJoinGame(ipInputField.text);
    }

    public void OnOptionsPressed()
    {
        OptionsMenu.Show();
    }

    public void OnExitPressed()
    {
        Application.Quit();
    }
}
