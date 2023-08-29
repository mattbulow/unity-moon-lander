using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Teleport : MonoBehaviour
{
    public GameObject terrain;
    public Transform spaceshipPos;  
    public Transform cameraPos;

    private float spaceshipPosX;
    private int length;

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Something entered trigger");

        length = terrain.GetComponent<MoonTerrain>().length;
        spaceshipPosX = spaceshipPos.position.x;
        if (spaceshipPosX > length)
            length = -length;

        spaceshipPos.position = new Vector3(spaceshipPosX + length, spaceshipPos.position.y, spaceshipPos.position.z);
        cameraPos.position = new Vector3(cameraPos.position.x + length, cameraPos.position.y, cameraPos.position.z);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Something entered collider");
    }

}
