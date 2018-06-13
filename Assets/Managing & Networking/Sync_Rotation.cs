using UnityEngine;
using UnityEngine.Networking;

public class Sync_Rotation : NetworkBehaviour
{
    // a custom script do synchronize rotation via players

    [SyncVar] private Quaternion syncPlayerRotation;

    void Update()
    {
        TransmitRotations();
        if (!isLocalPlayer)
        {
            transform.localRotation = syncPlayerRotation;
        }
    }

    [Command]
    void CmdProvideRotationsToServer(Quaternion playerRot)
    {
        syncPlayerRotation = playerRot;
    }

    [Client]
    void TransmitRotations()
    {
        if (isLocalPlayer)
        {
            CmdProvideRotationsToServer(transform.localRotation);
        }
    }
}