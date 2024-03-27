using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using static UnityEngine.GraphicsBuffer;

public class spaceshipControl : MonoBehaviour
{
    private Rigidbody2D rb;

    private bool isCollided;
    private float exhaustTimer = 0;
    private float flipTimer = 0;

    public SpriteRenderer exhaustRenderer;
    public GameObject explosion;
    public GameObject anchor;
    public Camera cam;

    public Vector3 spaceshipInitPos = new Vector3(120, 50, 0);
    public float moveForce = 10f; 
    public float rotateForce = 10f;
    public float sasForce = 20f; // deg/sec
    public float explodeSpeedThr = 0.5f;
    

    void initPosition()
    {
        transform.position = spaceshipInitPos;
        rb.rotation = 90;
        rb.velocity = new Vector2(15, 0);
        rb.angularVelocity = 0;

    }

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        initPosition();
        exhaustRenderer.enabled = false;
        explosion.SetActive(false);
    }

    void Update()
    {
        // this should happen in a class: "reset level"
        if (Input.GetKeyDown(KeyCode.Space))
        {
            initPosition();
            cam.transform.position = new Vector3(transform.position.x, cam.transform.position.y, cam.transform.position.z);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float torque = -Input.GetAxisRaw("Horizontal") * rotateForce;
        float thrust = Input.GetAxisRaw("Vertical") * moveForce;
        float maxExhaustScale = 2.5f;
        float minExhaustScale = 0.3f;
        float exhaustTime = 0.3f; //sec
        Vector3 scaleRate = new Vector3(1, 1, 1) * (maxExhaustScale - minExhaustScale) / exhaustTime; // unitPercent/sec
        Vector3 initScale = new Vector3(1, 1, 1) * minExhaustScale; // unitPercent

        if (thrust > 0)
        {           
            rb.AddRelativeForce(new Vector2(0, thrust));

            //Make thrust bigger if heald for longer
            exhaustTimer += Time.fixedDeltaTime;
            
            if (exhaustTimer > 0 && exhaustTimer < exhaustTime)
            {
                anchor.transform.localScale = initScale + scaleRate * exhaustTimer; 
            }

            exhaustRenderer.enabled = true;

            flipTimer += Time.fixedDeltaTime;
            if (flipTimer > 0.05)
            {
                Debug.Log("FlipTimer: " + flipTimer + "\nexhaustRenderer.flipX: " + exhaustRenderer.flipX);
                exhaustRenderer.flipX = !exhaustRenderer.flipX;
                flipTimer = 0;
            }

        }
        else
        {
            exhaustRenderer.enabled = false;
            exhaustTimer = 0;
        }

        if (torque != 0) {
            //transform.Rotate(0, 0, torque  * Time.fixedDeltaTime);
            //rb.rotation += -torque * Time.fixedDeltaTime;
            rb.AddTorque(torque);
        }
        else
        {
            if (!isCollided)
            {
                float angVel = rb.angularVelocity;
                // should take the current velocity and reduce it by some amount every frame
                // dont want to do this unless we are not colliding
                float angleVelNew = Mathf.Max(Mathf.Abs(angVel) - sasForce * Time.fixedDeltaTime, 0f);
                rb.angularVelocity = Mathf.Sign(angVel) * angleVelNew;
            }

        }

    }

    void OnCollisionEnter2D()
    {
        isCollided = true;

        Debug.Log("CollisionVelocity = " + rb.velocity.magnitude);
        if (rb.velocity.magnitude > explodeSpeedThr)
        {
            ExplodeShip();
        }
        else
        {
            // if also in a correct location then freeze game 'WIN'
            // this should be done by triggers of landing sites
        }

    }

    void OnCollisionExit2D()
    {
        isCollided = false;
    }

    void ExplodeShip()
    {
        explosion.SetActive(true);


        //TODO, reset/end game
    }

}
