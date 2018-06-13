using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(MeshRenderer))]
public class LaunchArcMesh : NetworkBehaviour
{
    [SerializeField] private int cannonNumber;  // the number of cannon "system", by default 0 - left, 1 - right, 2 - front, 3 - back
    [Tooltip("Will not update on runtime.")]
    [SerializeField] private int resolution = 200;      // how "round" should be the arc
    [SerializeField] private float meshWidth = 5f;

    private float angle;
    private float velocity;
    private float gravity;
    private float radianAngle;
    private CannonSystem cannonSystem;
    private Mesh mesh;
    private Ship ship;

    // this three arrays are init only once, because of garbage 
    private Vector3[] arcArray;
    private Vector3[] vertices;
    private int [] triangles;

    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        gravity = -Physics.gravity.y;
        ship = GetComponentInParent<Ship>();
        cannonSystem = ship.GetComponent<CannonSystem>();
        arcArray = new Vector3[resolution + 1];
        vertices = new Vector3[(resolution + 1) * 2];
        triangles = new int[(resolution) * 12];

    }

    void LateUpdate()
    {
        if(ship.isLocalPlayer && ship.CurrentShipState == Ship.ShipState.Swimming)
        {
            mesh.Clear();
            // we clear the mesh and need to do a new only if it's the active part
            if (cannonSystem.ActivePart == cannonNumber)
            {
                velocity = cannonSystem.ShootForce / 2f;
                angle = cannonSystem.ShootAngle / 4f * 180f;
                MakeArcMesh(CalculateArcArray());
            }
        }
    }

    private void MakeArcMesh(Vector3[] arcVerts)
    {
        for (int i = 0; i < resolution + 1; i++)
        {
            vertices[i * 2] = new Vector3(meshWidth * 0.5f, arcVerts[i].y, arcVerts[i].x);
            vertices[i * 2 + 1] = new Vector3(meshWidth * -0.5f, arcVerts[i].y, arcVerts[i].x);

            if (i != resolution)
            {
                triangles[i * 12] = i * 2;
                triangles[i * 12 + 1] = triangles[i * 12 + 4] = i * 2 + 1;
                triangles[i * 12 + 2] = triangles[i * 12 + 3] = (i + 1) * 2;
                triangles[i * 12 + 5] = (i + 1) * 2 + 1;

                triangles[i * 12 + 6] = i * 2;
                triangles[i * 12 + 7] = triangles[i * 12 + 10] = (i + 1) * 2;
                triangles[i * 12 + 8] = triangles[i * 12 + 9] = i * 2 + 1;
                triangles[i * 12 + 11] = (i + 1) * 2 + 1;

            }
        }
        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }

    private Vector3[] CalculateArcArray()
    {
        radianAngle = Mathf.Deg2Rad * angle;
        float maxDistance = Mathf.Pow(velocity, 2f) * Mathf.Sin(2 * radianAngle) / gravity;

        for (int i = 0; i < resolution + 1; i++)
        {
            float t = (float)i / (float)resolution;
            arcArray[i] = CalculateArcPoint(t, maxDistance);
        }

        return arcArray;
    }

    private Vector3 CalculateArcPoint(float t, float maxDistance)
    {
        float x = t * maxDistance;
        float y = x * Mathf.Tan(radianAngle) -
            (gravity * Mathf.Pow(x, 2f) / (2 * Mathf.Pow(velocity, 2) * Mathf.Pow(Mathf.Cos(radianAngle), 2)));

        return new Vector3(x, y, 0f);
    }
}