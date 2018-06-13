using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class HudManager : MonoBehaviour
{
    // hides the hud

    private CanvasGroup canvas;

	void Start ()
    {
        canvas = GetComponent<CanvasGroup>();
	}
	
	void Update ()
    {
		if(CrossPlatformInputManager.GetButtonDown("Hide HUD"))
		{
		    canvas.alpha = canvas.alpha == 1 ? 0 : 1;
		}
    }
}
