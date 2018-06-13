using UnityEngine;
using UnityEngine.Networking;

public class Cannonball : NetworkBehaviour
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private AudioClip boom;
    [SerializeField] private GameObject waterSplashProjectile;
    [SerializeField] private GameObject explosionProjectile;
    [SerializeField] private Player fatherPlayer;
    private bool deliveredDamage = false;

    private const float MAX_LIFETIME = 20f;

    public void Init(Player fatherPlayer)
    {
        this.fatherPlayer = fatherPlayer;
        if (fatherPlayer)
        {
            Physics.IgnoreCollision(GetComponent<SphereCollider>(), fatherPlayer.GetComponentInChildren<BoxCollider>(), true);
        }
        transform.parent = GameObject.FindGameObjectWithTag("CannonballOrganizer").transform;    // TODO to order in hierarchy
        // TODO implement object pool pattern
        Destroy(gameObject, MAX_LIFETIME);                                           // to be sure it will be destroyed
    }

    private void OnCollisionEnter(Collision collision)
    {
        Collider collider = collision.collider;
        if (collider.tag == "Water")
        {
            GetComponent<Collider>().enabled = false;
            SpawnWaterSplashProjectile(transform.position);
            Destroy(gameObject, 1f);
        }
        else if (!deliveredDamage && collider.GetComponentInParent<Health>())
        {
            AudioSource audioSource = collider.GetComponent<AudioSource>();
            if (audioSource)
            {
                audioSource.PlayOneShot(boom);
            }
            Health health = collider.GetComponentInParent<Health>();

            SpawnExplosionProjectile(transform.position);
            if (health.CurrentHealth > 0 && health.CurrentHealth - damage <= 0)
            {
                fatherPlayer.AddKill();
            }

            health.TakeDamage(damage);
            deliveredDamage = true;
            Destroy(gameObject, 2f);
        }
    }

    private void SpawnWaterSplashProjectile(Vector3 position)
    {
        Instantiate(waterSplashProjectile, position, Quaternion.identity);
    }

    public void SpawnExplosionProjectile(Vector3 position)
    {
        Instantiate(explosionProjectile, position, Quaternion.identity);
    }
}
