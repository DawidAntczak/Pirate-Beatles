using UnityEngine;
using UnityEngine.PostProcessing;
using UnityStandardAssets.CrossPlatformInput;

public class Lunette : MonoBehaviour
{
    // makes the player able to use the lunette
    // lunette uses diferent post processing profile (more vignette) and changes camera FOV

    // TODO the two post processing profiles should be identical excluding vignette, so I think, this requires a refactor
    // because changing post processing effects, requires to copy everything to the second profile (or copy profile and change vignette)

    private Ship ship;
    private float startFOV;
    private PostProcessingBehaviour postProcessingBehaviour;
    private PostProcessingProfile initialVignetteSettings;
    private Vector3 initialCameraPosition;
    private Vector3 initialCameraRotation;
    private bool zoomingEnabled = true;
    [SerializeField] private PostProcessingProfile zoomVignetteSettings;
    [SerializeField] private float zoomStrenght = 3f;
    private Camera mainCamera;

    void Start ()
    {
        ship = GetComponentInParent<Ship>();
        mainCamera = Camera.main;
        if (ship.isLocalPlayer)
        {
            startFOV = mainCamera.GetComponent<Camera>().fieldOfView;
            postProcessingBehaviour = mainCamera.GetComponent<PostProcessingBehaviour>();
            initialVignetteSettings = postProcessingBehaviour.profile;
            ship.OnOurShipDisabling += DisableZooming;             // if the ship sinks, we will immediately get out of lunette view
        }
    }
	
	void Update ()
    {
        if (ship.isLocalPlayer && CrossPlatformInputManager.GetButtonDown("Zoom") && ship.CurrentShipState == Ship.ShipState.Swimming)
        {
            ZoomControlling();
        }
    }

    private void ZoomControlling()
    {
        if (mainCamera.fieldOfView == startFOV)
        {
            ZoomIn();
        }
        else
        {
            ZoomOut();
        }
    }

    private void ZoomIn()
    {
        mainCamera.fieldOfView /= zoomStrenght;
        postProcessingBehaviour.profile = zoomVignetteSettings;
        initialCameraPosition = mainCamera.transform.localPosition;
        initialCameraRotation = mainCamera.transform.localEulerAngles;
        mainCamera.transform.position = transform.position;
    }

    private void ZoomOut()
    {
        mainCamera.fieldOfView = startFOV;
        postProcessingBehaviour.profile = initialVignetteSettings;

        mainCamera.transform.localPosition = initialCameraPosition;
        mainCamera.transform.localEulerAngles = initialCameraRotation;
    }

    private void DisableZooming()
    {
        if (mainCamera.fieldOfView != startFOV)
        {
            ZoomOut();
        }
        zoomingEnabled = false;
    }
}
