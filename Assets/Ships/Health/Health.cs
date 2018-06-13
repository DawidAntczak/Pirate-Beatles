using UnityEngine;
using UnityEngine.Networking;

public class Health : NetworkBehaviour
{
    [SerializeField] public float maxHealth = 100f;
    [SyncVar] private float currentHealth;
    private Healthbar healthbar;
    private MyHealthbar myHealthbar;

    private Ship ship;

    void Start ()
    {
        ship = GetComponent<Ship>();
        currentHealth = maxHealth;
        if (isLocalPlayer)
        {
            // if the ship with this script is our ship, we need to get the UI healthbar that we see everytime on screen
            myHealthbar = FindObjectOfType<MyHealthbar>();

        }
        else
        {
            // otherwise (if it's a ship of a other player), we need to get the healthbar above the ship
            healthbar = GetComponentInChildren<Healthbar>();
        }
    }

    private void Update()
    {
        if (isLocalPlayer)
        {
            // if the ship with this script is our ship, we need to update the UI healthbar that we see everytime on screen
            myHealthbar.UpdateMyHealthBar(currentHealth, maxHealth);
        }
        else
        {
            // otherwise (if it's a ship of a other player), we update the healthbar above the ship
            healthbar.UpdateHealthbar(currentHealth);
        }

        if (isServer && ship.CurrentShipState == Ship.ShipState.Swimming && currentHealth == 0f)
        {
            ship.RpcBreakShip();
        }
    }

    public void RenewHealth()
    {
        if (isServer)
        {
            currentHealth = maxHealth;
        }
    }

    public void TakeDamage(float damage)
    {
        if (!isServer)
            return;         // only the server should change the health value of ships and then send the values to others - [SyncVar]


        if (currentHealth > 0)
        {
            currentHealth -= damage;

            if (currentHealth < 0f)     // clamping
            {
                currentHealth = 0;
            }
        }
    }

    public float MaxHealth
    {
        get { return maxHealth; }
    }

    public float CurrentHealth
    {
        get { return currentHealth; }
        set { currentHealth = value; }
    }
}
