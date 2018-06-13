using UnityEngine;
using UnityEngine.Networking;

public class PlayerID : NetworkBehaviour
{
    // a script that creates and holds a unique ID for each player when they enter the server

    [SyncVar] public string playerUniqueName;
    [SerializeField] [SyncVar] private string playerNickname;
    private NetworkInstanceId playerNetID;

    public override void OnStartLocalPlayer()
    {
        GetNetIdentity();
        SetIdentity();
        CmdTellNickname(PlayerPrefsManager.GetPlayerNickname());
    }

    void Update()
    {
        if (transform.name == "" || transform.name == "Ship(Clone)")
        {
            SetIdentity();
        }
    }

    [Client]
    void GetNetIdentity()
    {
        playerNetID = GetComponent<NetworkIdentity>().netId;
        CmdTellServerMyIdentity(MakeUniqueIdentity());
    }

    void SetIdentity()
    {
        if (!isLocalPlayer)
        {
            transform.name = playerUniqueName;
        }
        else
        {
            // case when it is the local client player we need to create the ID
            name = MakeUniqueIdentity();
        }
    }

    string MakeUniqueIdentity()
    {
        return "Player" + playerNetID.ToString();
    }

    [Command]
    void CmdTellServerMyIdentity(string name)
    {
        playerUniqueName = name;
    }

    [Command]
    private void CmdTellNickname(string nickname)
    {
        if (nickname.Length > 0)
        {
            playerNickname = nickname;
        }
        else
        {
            playerNickname = playerUniqueName;
        }
    }

    public string PlayerNickname
    {
        get { return playerNickname; }
    }
}