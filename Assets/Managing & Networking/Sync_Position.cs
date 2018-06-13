using UnityEngine;
using UnityEngine.Networking;

public class Sync_Position : NetworkBehaviour
{
    // a custom script do synchronize position via players

    [SyncVar] private Vector3 syncPlayerPosition;
    [SerializeField] private float lerpRate = 15f;

    void Update()
    {
        TransmitPosition();
        if (!isLocalPlayer)
        {
            transform.position = Vector3.Lerp(transform.position, syncPlayerPosition, lerpRate * Time.deltaTime);
        }
    }

    [Command]
    void CmdProvidePositionToServer(Vector3 playerPos)
    {
        syncPlayerPosition = playerPos;
    }

    [Client]
    void TransmitPosition()
    {
        if (isLocalPlayer)
        {
            CmdProvidePositionToServer(transform.position);
        }
    }
}
