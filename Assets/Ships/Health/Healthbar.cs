using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    // it's the healthbar that we see above ships

    private float maxHealth;
    private Color minColor = Color.red;
    private Color maxColor = Color.green;
    private Image image;
    private float initialLength;
    private Camera mainCamera;

    private void Start()
    {
        Ship ship = GetComponentInParent<Ship>();
        if (ship.isLocalPlayer)
        {
            // we want not to see our healthbar above ship, we have it everytime on screen, we've done
            gameObject.SetActive(false);
        }
        image = GetComponent<Image>();
        maxHealth = ship.GetComponent<Health>().MaxHealth;
        initialLength = maxHealth / 150;

        mainCamera = Camera.main;
    }

    
    public void UpdateHealthbar(float currentHealth)
    {
        // we want the color to go from green to red and make the healthbar shrinking if health decreases

        // it should be already clamped in Health.cs, but for sure I will check
        if (currentHealth < 0f) { currentHealth = 0f; }

        float fraction = currentHealth / maxHealth;
        image.color = Color.Lerp(minColor, maxColor,
                                Mathf.Lerp(0, 1, currentHealth / maxHealth));

        transform.localScale = new Vector3(initialLength * fraction, transform.localScale.y, transform.localScale.z);
    }

    public void LateUpdate()
    {
        transform.LookAt(mainCamera.transform);
    }
}
