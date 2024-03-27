using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingPad : MonoBehaviour
{
    public Rigidbody2D spaceship;


    private void OnTriggerStay2D(Collider2D collision)
    {
        // If spaceship velocity is low and some time has passed, then WIN GAME!
        //timer
        if (spaceship.velocity.magnitude < 0.05)
        {
            Debug.Log("Landing Successful, you WIN!");
        }

    }

}
