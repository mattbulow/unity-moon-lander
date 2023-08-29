using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class spaceshipControl : MonoBehaviour
{
    public Vector3 spaceshipInitPos = new Vector3(120,50,0);

    private Rigidbody2D rb;

    public float moveForce = 10f; 
    public float rotateRate = 10f;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        transform.position = spaceshipInitPos;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.position = spaceshipInitPos;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float x_pos = Input.GetAxis("Horizontal") * moveForce;
        float y_pos = Input.GetAxis("Vertical") * moveForce;
        if (y_pos < 0) y_pos = 0;


        //transform.Rotate(0, 0, x_pos * rotateRate * Time.fixedDeltaTime);

        rb.rotation += x_pos * rotateRate * Time.fixedDeltaTime;

        // Want force to be alligned with rotation of craft

        rb.AddRelativeForce(new Vector2(0, y_pos));

    }
}
