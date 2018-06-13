using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    // it's the UI with the name of player above ship

    private PlayerID player;
    private Text text;
    private Camera mainCamera;

    void Start ()
	{
        player = GetComponentInParent<PlayerID>();
        mainCamera = Camera.main;

        if (player.isLocalPlayer)
	    {
	        gameObject.SetActive(false);
	    }
	    else
	    {
	        text = GetComponentInChildren<Text>();
        }
    }

	void Update ()
	{
        text.text = player.PlayerNickname;
        transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
                            mainCamera.transform.rotation * Vector3.up);
	}
}
