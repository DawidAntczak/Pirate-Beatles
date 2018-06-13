using UnityEngine;
using UnityEngine.Networking;

public class CloudSystem : NetworkBehaviour
{
    // a script attached to cloud system

    [SerializeField] Transform playerTransform = null;
    [SerializeField] float cloudSpeed = 1;
    private float distance = 10.0f;

    private float height = 0;
    private float heightDamping = 0;
    private float rotationDamping = 0;

    void Update()
    {
        transform.Rotate(0, Time.deltaTime * cloudSpeed, 0);
        // rotate 90 degrees around the object's local Y axis:
    }

    void LateUpdate()
    {
        if (playerTransform)
        {
            float wantedHeight = playerTransform.position.y + height;

            float currentHeight = transform.position.y;

            currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

            transform.position = playerTransform.position;

            transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);
        }
    }
}
