using UnityEngine;

public class BackgroundMove : MonoBehaviour
{
    [SerializeField]
    private float backgroundSpeed = 2.0f;

    [SerializeField]
    private float minX = -10f;  // Left bound of background movement
    [SerializeField]
    private float maxX = 10f;   // Right bound of background movement

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // Move background horizontally
        float newX = transform.position.x + backgroundSpeed * Time.deltaTime;

        // Clamp so background doesn't move out of the visible frame
        newX = Mathf.Clamp(newX, minX, maxX);

        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }
}
