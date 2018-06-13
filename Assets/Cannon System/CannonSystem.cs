using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityStandardAssets.CrossPlatformInput;

public class CannonSystem : NetworkBehaviour
{
    [SerializeField] private GameObject[] cannonParts;  // all cannon system parts (0 - left, 1 - right, 2 - front, 3 - back (if exist))
    [SerializeField] private Ship ship;
    [SerializeField] private float reloadingTime = 10f;
    [SerializeField] private float shootForce = 200f;
    [SerializeField] private GameObject cannonBallPrefab;
    private Image [] cannonsLoaderImages;               // all images on screen, that are showing loading state
    private float shootAngle = 0.5f;                    // 1 is 45 degrees, 0.5 is 22.5, ...
    private AudioSource audioSource;
    private int activePart;
    private Player owner;
    private const float CANNON_INACCURACY = 0.1f;       // TODO more ships, more cannon types, maybe change later for a serialized field
    private GameObject tmpGameObject;

    void Start ()
    {
        tmpGameObject = new GameObject();
        if (!isLocalPlayer)
            return;

        activePart = 0;
        ship = GetComponent<Ship>();
        owner = GetComponent<Player>();
        //TODO have only one audio source on ship and take the sound from the type of cannonball
        audioSource = GetComponents<AudioSource>()[1];
        cannonsLoaderImages = new Image[cannonParts.Length];
        cannonsLoaderImages[0] = GameMenu.Instance.LeftLoader;
        cannonsLoaderImages[1] = GameMenu.Instance.RightLoader;
        // in this moment, no ship has front and back cannons, later...
        ResetSliders();
        cannonsLoaderImages[activePart].color = Color.green;
    }

    void Update()
    {
        if (!isLocalPlayer || ship.CurrentShipState != Ship.ShipState.Swimming)
            return;

        // TODO maybe some final strings
        shootAngle = Mathf.Clamp(shootAngle + CrossPlatformInputManager.GetAxis("Angle Control"), 0.05f, 1f);

        if (CrossPlatformInputManager.GetButtonDown("Left Cannons"))
        {
            activePart = 0;
        }
        else if (CrossPlatformInputManager.GetButtonDown("Right Cannons"))
        {
            activePart = 1;
        }

        UpdateSliders();

        // the shot button shoots with current chosen cannon part (if it's posible to shoot)
        // the mouse buttons should immediately change (if it's posible to shoot) the cannon part and shoot
        // when there are more cannon parts, we need a better mouse ( ͡° ͜ʖ ͡°)
        if (CrossPlatformInputManager.GetButtonDown("Shot")
                            && cannonsLoaderImages[activePart].fillAmount == 1f)
        {
            audioSource.PlayOneShot(audioSource.clip);
            Shoot();
        }
        else if(Input.GetKeyDown(KeyCode.Mouse0) && cannonsLoaderImages[0].fillAmount == 1f)
        {
            audioSource.PlayOneShot(audioSource.clip);
            activePart = 0;
            Shoot();
        }
        else if (Input.GetKeyDown(KeyCode.Mouse1) && cannonsLoaderImages[1].fillAmount == 1f)
        {
            audioSource.PlayOneShot(audioSource.clip);

            activePart = 1;
            Shoot();
        }
    }

    private void Shoot()
    {
        // I have no idea how to implement this properly with UNet
        // TODO implement object pool pattern, maybe helps

        GameObject shootingCannon;
        foreach (Cannon cannon in cannonParts[activePart].GetComponentsInChildren<Cannon>())
        {
            shootingCannon = cannon.gameObject;
            CmdSpawnBalls(shootingCannon.transform.position, shootingCannon.transform.rotation, shootAngle, owner.gameObject);
            if (isServer)
            {
                SpawnBalls(shootingCannon.transform.position, shootingCannon.transform.rotation, shootAngle, owner.gameObject);
            }
            else
            {
                CmdSpawnBalls(shootingCannon.transform.position, shootingCannon.transform.rotation, shootAngle, owner.gameObject);
            }
        }

        // now unloading the cannons
        cannonsLoaderImages[activePart].fillAmount = 0f;
        cannonsLoaderImages[activePart].color = Color.red;
    }

    [Command]
    public void CmdSpawnBalls(Vector3 position, Quaternion rotation, float angle, GameObject fatherPlayer)
    {
        SpawnBalls(position, rotation, angle, fatherPlayer);
    }

    private void SpawnBalls(Vector3 position, Quaternion rotation, float angle, GameObject fatherPlayer)
    {
        Transform cannonTranform = tmpGameObject.transform;
        cannonTranform.SetPositionAndRotation(position, rotation);

        GameObject ball = Instantiate(cannonBallPrefab, cannonTranform.position, Quaternion.identity);
        ball.GetComponent<Cannonball>().Init(fatherPlayer.GetComponent<Player>());

        Vector3 direction = cannonTranform.forward;
        ball.GetComponent<Rigidbody>().velocity =
                    new Vector3(direction.x * Random.Range(1 - CANNON_INACCURACY, 1 + CANNON_INACCURACY),
                                angle * Random.Range(1 - CANNON_INACCURACY, 1 + CANNON_INACCURACY),
                                direction.z * Random.Range(1 - CANNON_INACCURACY, 1 + CANNON_INACCURACY)).normalized * shootForce;
        NetworkServer.Spawn(ball);
    }

    public void ResetSliders()
    {
        foreach (Image image in cannonsLoaderImages)
        {
            image.fillAmount = 1f;
        }

        shootAngle = 0.5f;
    }

    private void UpdateSliders()
    {
        for (int i = 0; i < cannonsLoaderImages.Length; i++)
        {
        // TODO actualy no need to add more if it's already filled but we need to handle changing color by choosing part anywhere else
            cannonsLoaderImages[i].fillAmount += Time.deltaTime / reloadingTime;
            if (cannonsLoaderImages[i].fillAmount == 1f)
            {
                if (i == activePart)
                {
                    cannonsLoaderImages[i].color = Color.green;
                }
                else
                {
                    cannonsLoaderImages[i].color = Color.white;
                }
            }
        }
    }

    public float ShootForce
    {
        get { return shootForce; }
    }

    public int ActivePart
    {
        get { return activePart; }
    }

    public float ShootAngle
    {
        get { return shootAngle; }
    }
}