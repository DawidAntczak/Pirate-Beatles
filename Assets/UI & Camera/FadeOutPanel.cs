using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOutPanel : MonoBehaviour
{
    [SerializeField] private float timeInSeconds = 2f;
    private float howManyInSecond;
    private RawImage image;

    void Start ()
    {
        image = GetComponent<RawImage>();
        howManyInSecond = 255f / timeInSeconds;
    }
	
	void Update ()
    {
        if (image.color.a > 0f)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b,
                image.color.a - (howManyInSecond * Time.deltaTime));
        }
        else
        {
            Destroy(gameObject);
        }
	}
}
