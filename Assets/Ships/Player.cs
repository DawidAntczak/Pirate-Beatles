using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    // collects player stats from log in, to log out from server
    // the ship should not collect the player stats, because I'm going to add a possibility to change ships
    // (when I only had more working models...)

    [SyncVar] private int kills = 0;
    [SyncVar] private int deaths = 0;

    public void AddDeath()
    {
        if (isServer)
        {
            deaths++;
        }
    }

    public void AddKill()
    {
        if (isServer)
        {
            kills++;
        }
    }

    public int Deaths
    {
        get { return deaths; }
    }

    public int Kills
    {
        get { return kills; }
    }
}
