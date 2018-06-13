using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    // component that destroys game object after an amount of time
    
    [SerializeField] private float timeToDestroyAfter = 2f;

	void Start ()
	{
	    Destroy(gameObject, timeToDestroyAfter);
	}
}
