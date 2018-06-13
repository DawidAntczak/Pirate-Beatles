using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.Networking;

public class SailingController : NetworkBehaviour
{
    // TODO I think I should detach some ship behaviors that are connected with input from Ship.cs and add here

    [SerializeField] private float lerpSpeed = 10f;
    private Ship ship;

    void Start()
    {
        ship = GetComponent<Ship>();
    }

    void Update()
    {
        if (!isLocalPlayer)
            return;

        if (ship.CurrentShipState == Ship.ShipState.Swimming)
        {
            SailsControlling();
            RudderControlling();
        }
    }

    private void RudderControlling()
    {
        float axis = CrossPlatformInputManager.GetAxis("Horizontal") * ship.RudderDelta * Time.deltaTime;

        if(axis == 0)
        {
            // makes the rudder self go to the center position if there is no input
            ship.Rudder = Mathf.Lerp(ship.Rudder, 0f, Time.deltaTime * lerpSpeed);
        }
        else
        {
            ship.Rudder = Mathf.Clamp(ship.Rudder + axis, -ship.MaxRudder, ship.MaxRudder);
        }
    }

    private void SailsControlling()
    {
        // TODO no keycodes
        if (Input.GetKeyDown(KeyCode.S) && ship.FullSails)
        {
            ship.PullUpSaills();
        }
        else if (Input.GetKeyDown(KeyCode.W) && !ship.FullSails)
        {
            ship.PullDownSaills();
        }
    }
}