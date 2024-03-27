using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 1f;
    private Vector3 velocity = Vector3.zero;
    public int orthoSize = 30;

    private BoxCollider2D boxCollider;
    public Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        cam.orthographicSize = orthoSize;
        transform.position = new Vector3(target.position.x, orthoSize, -10);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Define target position as the x location of the target (space ship)
        Vector3 targetPosition = new Vector3(target.transform.position.x, transform.position.y, transform.position.z);

        // Smoothly move the camera towards that target position
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

        // Set box collider size
        Vector3 bottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 topRight = cam.ViewportToWorldPoint(new Vector3(1, 1, 0));
        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.size = (new Vector2(topRight.x - bottomLeft.x + 3, topRight.y - bottomLeft.y + 3));

    }
}
