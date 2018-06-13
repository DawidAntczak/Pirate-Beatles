using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.Networking;

public class MouseLooking : NetworkBehaviour
{
    // gives the ability to look around

    [SerializeField] private float XSensitivity = 1f;
    [SerializeField] private float YSensitivity = 1f;
    [SerializeField] private float scrollingSensitivity = 3f;
    private float rotationX;
    private GameObject player;
    private float startFOV;
    private Camera myCamera;

    public override void OnStartLocalPlayer()
    {
        YSensitivity /= 30f;
        myCamera = GetComponent<Camera>();
        startFOV = myCamera.fieldOfView;
        rotationX = transform.eulerAngles.x;
    }

    void LateUpdate()
    {
        if(player)
        {
            Rotate();
            //Scroll();
        }
    }

    private void Rotate()
    {
        float xRot = CrossPlatformInputManager.GetAxis("Mouse X") * XSensitivity;
        float yRot = CrossPlatformInputManager.GetAxis("Mouse Y") * YSensitivity;

        transform.RotateAround(player.transform.position, Vector3.up, xRot);

        rotationX = Mathf.Clamp(rotationX - yRot, -20, 20);

        transform.localEulerAngles = new Vector3(rotationX, transform.localEulerAngles.y, transform.eulerAngles.z);
    }


    private void Scroll()
    {
        // would be nice to enable scrolling but first we need a second scrolling wheel on the mouse

        /*Vector3 direction = transform.forward;
        float scrolling = CrossPlatformInputManager.GetAxis("Mouse ScrollWheel");

        Vector3 newPosition = new Vector3(transform.position.x + scrolling * direction.x * scrollingSensitivity,
                                    transform.position.y + scrolling * direction.y * scrollingSensitivity,
                                    transform.position.z + scrolling * direction.z * scrollingSensitivity);

        float distanceFromShip = Vector3.Distance(Player.transform.position, newPosition);

        if (distanceFromShip < 90f && scrolling < 0f)
        {
            transform.position = newPosition;
        }
        else if(distanceFromShip > 48f && scrolling > 0f)
        {
            transform.position = newPosition;
        }*/
    }


    public GameObject Player
    {
        set { player = value; }
    }
}
