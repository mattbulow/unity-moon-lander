using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D;

public class MoonTerrain : MonoBehaviour
{

    public int length = 200;

    public int smoothCount = 2;
    public int smoothRadius = 3;
    public int maxHeight = 80;
    public int[] landingSize = {10, 10, 6};

    public Transform leftTrigger;
    public Transform rightTrigger;

    private SpriteShapeController spriteShapeCtrl;
    private Spline spline;

    private float minHeightActual;
    private float[] heightmap;

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
        spline.InsertPointAt(1, new Vector3(length * 2 - 1, 0, 0));
        for (int i = 0; i < length * 2; i++)
        {
            spline.InsertPointAt(i + 1, new Vector3(i, 1, 0));
            spline.SetTangentMode(i + 1, ShapeTangentMode.Continuous);
        }
        spline.SetTangentMode(1, ShapeTangentMode.Linear);
        spline.SetTangentMode(length * 2, ShapeTangentMode.Linear);


        heightmap = new float[length];

        //make height map
        for (int i = 0; i < length; i++)
        {
            heightmap[i] = UnityEngine.Random.Range(1, maxHeight);
            // add points with height from map
            spline.SetPosition(i + 1, new Vector3(i, heightmap[i], 0));
        }

        for (int i = 0; i < smoothCount; i++)
            SmoothTerrain();

        //transform whole map down by the height of lowest point
        for (int i = 0; i < length; i++)
        {
            float newYpos = spline.GetPosition(i + 1).y - minHeightActual + 1;
            spline.SetPosition(i + 1, new Vector3(i, newYpos, 0));
            heightmap[i] = newYpos;
        }

        int[] landingX = new int[3] {0,0,0};
        List<int> offLimits = new List<int>();
        offLimits.Clear();
        // add landing pads
        for (int i = 0; i < landingSize.Length; i++)
        {
            landingX[i] = UnityEngine.Random.Range(1, length - landingSize[i]);

            // create an array of x values that are off limits by checking previous landingX sites
            for (int m = 0; m < i; m++)
            {
                offLimits.AddRange(Enumerable.Range(landingX[m] - 2, landingX[m] + landingSize[m] + 2));
            }
            // if landingx[i] is equal to any value in the offLimits list then make a new random number
            bool offLimitFlag = true;
            while (offLimitFlag)
            {
                foreach (int value in offLimits)
                {
                    if (landingX[i] == value) 
                    {
                        // if value is off limits then recalculate a new landing pad X value and re-check
                        landingX[i] = UnityEngine.Random.Range(1, length - landingSize[i]);
                        break;
                    }
                }
                offLimitFlag = false;
            }
            
            //TODO: reject random numbers that are too close to other landing spots
            // while landingX[i] is close to landingX[i-1], generate a new number

            // get next x number of nodes and find average height, and use that instead.
            float[] landingY = new float[landingSize[i]];
            Array.Copy(heightmap, landingX[i], landingY, 0, landingSize[i]);

            // make this spot and next n spots flat
            for (int n = 0;n < landingSize[i]; n++)
            {
                spline.SetPosition(landingX[i] + n, new Vector3(landingX[i] + n-1, landingY.Average(), 0));
            }
            
            // make new game object, set as child, add collider, make trigger, add color


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
            heightmap[i] = heightAverage;

            //TODO, set tangent based on neighbours location
            //spline.SetRightTangent(i+1, new Vector3(0.1f,0,0));
            //spline.SetLeftTangent(i+1, new Vector3(-0.1f, 0, 0));
        }
    }


}
