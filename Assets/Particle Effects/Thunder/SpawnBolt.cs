using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBolt : MonoBehaviour
{
    // should spawn a bolt, to simulate thunder, quickly done to check some things

    // three magic properties, that make the bolt spawn in right positions
    [SerializeField] private float sumOfDistanceInXandY = 4000f;
    [SerializeField] private Vector3 mapCenter = new Vector3(500f, 100f, 500f);
    [SerializeField] private float height = 2000f;

    [SerializeField] private AudioClip [] audioClips;           // some different audio clips for bolt
    private AudioSource audioSource;
    private ParticleSystem particleSys;
    private float duration;                                     // how long should be on sky (the particle system main duration)
    private float lastPositionChange;                           // the last time it spawned

    void Start ()
    {
        audioSource = GetComponent<AudioSource>();
        particleSys = GetComponent<ParticleSystem>();
        duration = particleSys.main.duration;
        mapCenter += Vector3.up * height;

        Shoot();
    }

    void Update ()
	{
	    if (lastPositionChange + duration < Time.time)
        {
            Shoot();
	    }
	}

    private void Shoot()
    {
        // a problem with self stoping particle system will not be longer a problem if you stop it first
        particleSys.Stop();
        particleSys.Play();
        transform.position = RandomVector3();
        lastPositionChange = Time.time;
        // TODO sometimes the sound seems to be truncated
        // it's because before the sound ends, the bolt shoots the second time and changes position of game object
        // because the audio source is 3D, changing of the game object position, changes the volume that we here from ship
        // there should be a temporary game object holding the audio source and waiting to clips end
        // but the whole system would be done quite differently (will allow more bolts in one time), so no worry
        audioSource.PlayOneShot(audioClips[Random.Range(0, audioClips.Length)]);
    }

    private Vector3 RandomVector3()         // returns a "random" Vector3, that will be fine for our purpose
    {
        Vector3 normalizedVector3 = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
        Vector3 vector3 = normalizedVector3 * sumOfDistanceInXandY * Random.Range(0.5f, 2f);
        return vector3 + mapCenter;
    }
}
