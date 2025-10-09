using UnityEngine;

public class SimpleParallax : MonoBehaviour
{
    public Transform target;      // The player or camera
    public float parallaxFactor = 0.5f; // 0 = doesn't move, 1 = same speed as target

    private Vector3 initialPosition;

    void Start()
    {
        if (!target)
            target = Camera.main.transform; // default to camera if no target
        initialPosition = transform.position;
    }

    void Update()
    {
        Vector3 newPos = initialPosition + (target.position * parallaxFactor);
        transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
    }
}
