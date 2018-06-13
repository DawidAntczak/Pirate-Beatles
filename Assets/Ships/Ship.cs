using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Ship : NetworkBehaviour
{
    // this script should only describe ship specification
    // unfortunately it is connected with player
    // and has to be separated from typical player properties

    public delegate void ShipDisabled();
    public event ShipDisabled OnOurShipDisabling;

    [SerializeField] private float rotationDivider = 10f;
    [SerializeField] private bool fullSails = true;
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float inertia = 0.5f;
    [SerializeField] private  float maxspeed = 3f;
    [SerializeField] private GameObject shipCollider;
    private Player player;
    private Text respawnTimer;
    private const float totalRespawnTime = 10f;
    private float rudder = 0.0f;
    private float heading = 0.0f;
    private float rudderDelta = 90.0f;
    private const float maxRudder = 90.0f;

    // to bobing -------------------------------
    private float elapsed = 0.0f;
    private float seaLevel = 0.0f;
    private const float bobFrequency = 0.45f;
    private const float bob = 0.75f;
    //------------------------------------------

    private Animator animator;
    private Health health;
    private AudioSource audioSource;
    private Rigidbody shipRigidbody;
    private const float DAMAGE_PER_SECOND_IN_COLLISION = 10f;

    public enum ShipState
    {
        Swimming,
        Disabled
    };
    private ShipState currentShipState;
    private bool isRespawning = false;

    private const string BREAK_ANIM = "break";
    private const string SAILS_UP_ANIM = "sailsUp";
    private const string UNDER_WAY_ANIM = "UnderWay";



    void Start()
    {
        player = GetComponent<Player>();
        shipRigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        health = GetComponent<Health>();
        seaLevel = transform.position.y;
        animator = GetComponent<Animator>();

        respawnTimer = GameMenu.Instance.RespawnTimer;

        currentShipState = ShipState.Swimming;
    }

    public override void OnStartLocalPlayer()
    {
        if (isLocalPlayer)
        {
            Camera mainCamera = Camera.main;
            mainCamera.GetComponent<MouseLooking>().Player = gameObject;
            mainCamera.GetComponentInParent<CameraFollower>().Player = gameObject;
        }
    }

    void Update()
    {
        //TODO unfreeze rotation in future
        /*float angleX = Vector3.Angle(transform.up, Vector3.forward);
        float angleZ = Vector3.Angle(transform.up, Vector3.right);
        if ((angleX < 60 || angleX > 120 || angleZ < 60 || angleZ > 120) && !isDead)
        {
        }*/

        Bobbing();

        if(isLocalPlayer && currentShipState == ShipState.Disabled && !isRespawning)
        {
            isRespawning = true;
            StartCoroutine(HandleRespawning());
        }      
    }

    private void FixedUpdate()
    {
        if (currentShipState == ShipState.Swimming)
        {
            AddForceToMoveShip();
        }
    }

    private IEnumerator HandleRespawning()
    {
        float currentRespawnTime = totalRespawnTime;
        while (currentRespawnTime > 0f)
        {
            currentRespawnTime -= Time.deltaTime;
            respawnTimer.text = Mathf.Ceil(currentRespawnTime).ToString();
            yield return null;
        }

        if (isServer)
        {
            RpcRespawn();
        }
        else
        {
            CmdRespawn();
        }
    }

    private void Bobbing()
    {
        // some moving up and down independent from the waves
        // TODO maybe made it dependent from the waves? can be expensive
        elapsed += Time.deltaTime;
        float tempY = seaLevel + bob * Mathf.Sin(elapsed * bobFrequency * (Mathf.PI * 2));
        transform.position = new Vector3(transform.position.x, tempY, transform.position.z);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Terrain" || collision.gameObject.GetComponent<Health>())
        {
            speed /= 2f;        // TODO with the unfreezed rotation, it should be dealed better
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (currentShipState == ShipState.Swimming)
        {
            if (collision.gameObject.tag == "Terrain")
            {
                health.TakeDamage(DAMAGE_PER_SECOND_IN_COLLISION * Time.deltaTime);
            }
            if (collision.gameObject.GetComponent<Health>())
            {
                health.TakeDamage(DAMAGE_PER_SECOND_IN_COLLISION * Time.deltaTime);
            }

            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
    }

    private void AddForceToMoveShip()
    {
        heading = (heading + rudder * Time.deltaTime * SignedSqrt(speed) / rotationDivider) % 360;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, heading, transform.eulerAngles.z);

        float someMultiplier = (2f * maxRudder - Mathf.Abs(rudder)) / (2f * maxRudder);  //TODO change this system
        float targetSpeed = maxspeed * someMultiplier;
        if (!fullSails)
        {
            targetSpeed /= 3f;
        }
        float ratio = Mathf.Abs(targetSpeed - speed) / targetSpeed;
        speed = Mathf.Lerp(speed, targetSpeed, Time.deltaTime / inertia * ratio);

        shipRigidbody.AddRelativeForce(speed * Time.deltaTime * 10000f, 0, 0, ForceMode.Acceleration);
    }

    private float SignedSqrt(float x)
    {
        float r = Mathf.Sqrt(Mathf.Abs(x));
        if (x < 0)
        {
            return -r;
        }
        else
        {
            return r;
        }
    }

    [ClientRpc]
    public void RpcBreakShip()
    {
        player.AddDeath();
        currentShipState = ShipState.Disabled;
        animator.SetTrigger(BREAK_ANIM);

        if (isLocalPlayer)
        {
            OnOurShipDisabling();
        }
    }

    public void PullUpSaills()
    {
        fullSails = false;
        animator.SetBool(SAILS_UP_ANIM, true);
    }

    public void PullDownSaills()
    {
        fullSails = true;
        animator.SetBool(SAILS_UP_ANIM, false);
    }

    [ClientRpc]
    private void RpcRespawn()
    {
        Respawn();
    }

    [Command]
    private void CmdRespawn()
    {
        RpcRespawn();
    }

    private void Respawn()
    {   // TODO this respawn process should be quite different when I add more ships
        health.RenewHealth();
        if (isLocalPlayer)
        {
            Transform spawn = FindRespawnPosition();
            transform.rotation = Quaternion.identity;
            transform.position = spawn.position;
            GetComponent<CannonSystem>().ResetSliders();
            animator.Play(UNDER_WAY_ANIM);
            if (!fullSails)
            {
                PullDownSaills();
            }
            isRespawning = false;
            respawnTimer.text = "";
        }
        currentShipState = ShipState.Swimming;
        speed = 1f;
    }

    private Transform FindRespawnPosition()
    {
        // TODO a spawn position should be choosen where we not collide with other ship (colliders on spawn positions to check this)
        MyNetworkManager netManager = FindObjectOfType<MyNetworkManager>();
        List<Transform> startPositions = NetworkManager.singleton.startPositions;
        int pos = netManager.LastPosition = (netManager.LastPosition + 1) % startPositions.Count;
        Transform spawn = startPositions[pos];
        return spawn;
    }



    public bool FullSails
    {
        get { return fullSails; }
    }

    public float RudderDelta
    {
        get { return rudderDelta; }
    }

    public float MaxRudder
    {
        get { return maxRudder; }
    }

    public float Rudder
    {
        get { return rudder; }
        set { rudder = value; }
    }

    public ShipState CurrentShipState
    {
        get { return currentShipState; }
    }
}
