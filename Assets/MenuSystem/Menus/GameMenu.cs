using UnityEngine;
using UnityEngine.UI;

public class GameMenu : SimpleMenu<GameMenu>
{
    [SerializeField] private GameObject scoreboard;
    [SerializeField] private Text respawnTimer;
    [SerializeField] private Text playerNameText;
    [SerializeField] private Image leftLoader;
    [SerializeField] private Image rightLoader;
    [SerializeField] private MyHealthbar healthbar;

    private void Start()
    {
        playerNameText.text = PlayerPrefsManager.GetPlayerNickname();
    }

    private void Update()
    {
        // no keycodes
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            scoreboard.SetActive(true);
        }
        else if(Input.GetKeyUp(KeyCode.Tab))
        {
            scoreboard.SetActive(false);
        }
    }

    public Text RespawnTimer
    {
        get { return respawnTimer; }
    }

    public Image LeftLoader
    {
        get { return leftLoader; }
    }

    public Image RightLoader
    {
        get { return rightLoader; }
    }

    public MyHealthbar Healthbar
    {
        get { return healthbar; }
    }
}