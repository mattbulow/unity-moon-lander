using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class MoonTerrain : MonoBehaviour
{

    public int length = 200;

    public int smoothCount = 2;
    public int smoothRadius = 3;
    public int maxHeight = 80;

    public Transform leftTrigger;
    public Transform rightTrigger;

    private SpriteShapeController spriteShapeCtrl;
    private Spline spline;

    private float minHeightActual;

    void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        spriteShapeCtrl = GetComponent<SpriteShapeController>();
        spline = spriteShapeCtrl.spline;

        GenerateTarrain();

        leftTrigger.position = new Vector3(-3, leftTrigger.position.y, 0);
        rightTrigger.position = new Vector3(length*2+2, rightTrigger.position.y, 0);    

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GenerateTarrain();
        }
        //TODO: if key left right is pressed then move camera/spaceship
        // if spaceship reaches limit, teleport to other side of map
        // if spaceship gets close to ground, zoom in
    }

    void GenerateTarrain()
    {
        spline.Clear();
        spline.InsertPointAt(0, new Vector3(0, 0, 0));
        spline.InsertPointAt(1, new Vector3(length*2 - 1, 0, 0));
        for (int i = 0; i < length*2; i++)
        {
            spline.InsertPointAt(i + 1, new Vector3(i, 1, 0));
            spline.SetTangentMode(i + 1, ShapeTangentMode.Continuous);
        }
        spline.SetTangentMode(1, ShapeTangentMode.Linear);
        spline.SetTangentMode(length * 2, ShapeTangentMode.Linear);


        float[] heightmap = new float[length];

        //make height map
        for (int i = 0; i < length; i++)
        {
            heightmap[i] = Random.Range(1, maxHeight);
            // add points with height from map
            spline.SetPosition(i + 1, new Vector3(i, heightmap[i], 0));
        }
     
        for (int i = 0; i < smoothCount; i++)
            SmoothTerrain();

        //transform whole map down by the height of lowest point
        for (int i = 0; i < length; i++)
        {
            spline.SetPosition(i + 1, new Vector3(i, spline.GetPosition(i + 1).y - minHeightActual + 1, 0));
        }

        //add duplicate of map to spline
        for (int i = 0; i < length; i++)
        {
            spline.SetPosition(i + length + 1, new Vector3(i + length, spline.GetPosition(i + 1).y, 0));
        }

    }

    void SmoothTerrain()
    {
        minHeightActual = maxHeight;
        int SCAN_WIDTH = smoothRadius * 2 + 1;
        int m = 0;
 
        for (int i = 0; i < length; i++)
        {
            float heightSum = 0;

            for (int n = -smoothRadius; n <= smoothRadius; n++)
            {
                if (i+n < 0)
                    m = length + n;
                else if (i+n >= length)
                    m = n - 1;
                else
                    m = i + n; 
                
                float heightOfNeighbour = spline.GetPosition(m+1).y;
                heightSum += heightOfNeighbour;           
            }

            float heightAverage = heightSum / SCAN_WIDTH;
            if (heightAverage < minHeightActual)
            {
                minHeightActual = heightAverage;
                //Debug.Log("index = " + i + " minHeightActual: " + minHeightActual);
            }
                
            spline.SetPosition(i+1, new Vector3(i,heightAverage,0));

            //TODO, set tangent based on neighbours location
            //spline.SetRightTangent(i+1, new Vector3(0.1f,0,0));
            //spline.SetLeftTangent(i+1, new Vector3(-0.1f, 0, 0));
        }
    }


}
