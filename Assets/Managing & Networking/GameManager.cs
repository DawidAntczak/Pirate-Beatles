using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // maybe will be doing something is the future

	void Start ()
	{
	    DontDestroyOnLoad(gameObject);
        SceneManager.LoadScene(1);
    }
}
