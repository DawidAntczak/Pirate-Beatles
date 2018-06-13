using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    // script on the "stick", follows the player maintaining the distance

    private GameObject player;
    private Vector3 lastPosition;
	
	void LateUpdate ()
    {
        if (player)
        {
            transform.position = player.transform.position;
        }
    }

    public GameObject Player
    {
        set { player = value; }
    }
}
