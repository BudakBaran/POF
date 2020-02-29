using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellDrawer : MonoBehaviour
{
    public bool showCell = false;
    public bool showSurfaceCell = false;

    [Range(1,10000)]
    public int CubicSize = 5;
    public float Spacing = 1.0f;
  
    public float CalculateGridLength(float x, float y, float z) // if AABB dimensions are dynamic we have to find cell egde lenths in every frame.
    {
        float result;

        float volume = x * y * z; 

        result = Mathf.Pow(volume, -3);
        result = Mathf.Floor(result);

        return result;
    }

    public bool isSurfaceCell() // function will take cell id
    {
        bool result=false;
        // make sure if the cell is surface particle and send the result.
        // if ( Surface_cell == true) )
        {
            result = true;
        }

        return result;
        
    }

    void drawVoxels()
    {
        for (float i = 0; i < CubicSize; i = i + Spacing) // Boundaries will be replaced with cells 
        {
            for (float j = 0; j < CubicSize; j = j + Spacing)
            {
                for (float k = 0; k < CubicSize; k = k + Spacing)
                {
                    // This if condition is just a prototype to show how it will look
                    if (( i==0 || j==0 || k==0 || i== CubicSize-1 || j== CubicSize-1 || k== CubicSize-1)  && showSurfaceCell == true ) 
                    {
                        // if cell is on the surface make wirecube color red.
                        Gizmos.color = Color.red;
                    }
                   
                    else
                    {
                        Gizmos.color = Color.blue;
                    }

                    Gizmos.DrawWireCube(new Vector3(i, j, k), new Vector3(1, 1, 1));

                }
            }
        }
    }
    public void OnDrawGizmos()
    {
        if (showCell)
        {
            drawVoxels();
        }

    }


    void Start()
    {
        
        
    }

    void Update()
    {
        
    }
}
