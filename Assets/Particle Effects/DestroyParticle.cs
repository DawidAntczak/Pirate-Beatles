using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticle : MonoBehaviour
{
    // script destroys object with particle system after particle duration

	void Start ()
	{
	    Destroy(gameObject, GetComponent<ParticleSystem>().main.duration);
	}
}
