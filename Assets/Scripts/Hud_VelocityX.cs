using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;

public class Hud_VelocityX : MonoBehaviour
{
    public Rigidbody2D spaceship;
    public Text text;

    // Update is called once per frame
    void LateUpdate()
    {

        text.text = String.Format("Velocity X: {0:f1} m/s\nVelocity Y: {1:f1} m/s\nRotation: {2:f0} deg/s",
            spaceship.velocity.x, spaceship.velocity.y, spaceship.angularVelocity);

    }
}
