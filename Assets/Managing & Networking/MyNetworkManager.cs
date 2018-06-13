using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MyNetworkManager : NetworkManager
{
    public static MyNetworkManager Instance { get; private set; }

    private int lastSpawnPosition = 0;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        // TODO no keycodes
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (NetworkServer.active || NetworkClient.active)
            {
                Disconnect();
            }
        }
    }

    public void MyStartHost()
    {
        StartHost();
    }

    public void MyJoinGame(string ipAdress)
    {
        SetIPAdress(ipAdress);
        SetPort();
        StartClient();
    }

    private void SetIPAdress(string ipAdress)
    {
        PlayerPrefsManager.SetLastIP(ipAdress);
        networkAddress = ipAdress;
    }

    private void Disconnect()
    {
        StopHost();
        StopClient();
        StopServer();
    }

    private void SetPort()
    {
        networkPort = 7777;
    }

    public int LastPosition
    {
        get { return lastSpawnPosition; }
        set { lastSpawnPosition = value;}
    }
}
