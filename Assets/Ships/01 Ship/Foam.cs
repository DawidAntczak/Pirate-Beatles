using UnityEngine;

public class Foam : MonoBehaviour
{

    [SerializeField] private float scrollSpeed = 0.5f;
    [SerializeField] private Renderer renderer;

	void Start() 
    {
        renderer = GetComponent<Renderer>();
	}
	void Update()
    {
		float offset = Time.time * scrollSpeed;
        renderer.material.SetTextureOffset("_MainTex", new Vector2(0, offset));

	}
}