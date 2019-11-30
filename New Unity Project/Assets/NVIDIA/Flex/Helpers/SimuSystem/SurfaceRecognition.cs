using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceRecognition
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public int getFive()
    {
        return 5;
    }
    public float FindDistance(Vector3 vertex, Vector3 point)
    {
        return (float)Math.Sqrt(Math.Pow((vertex.x - point.x), 2) + Math.Pow((vertex.y - point.y), 2) + Math.Pow((vertex.z - point.z), 2));
    }

    public float FindConstant(float radius)
    {
        float cons = 3 / (2 * (float)(Math.PI * Math.Pow(radius, 3)));
        return cons;
    }
    public Vector3 FindGradientWeight(Vector3 particle, Vector3 neighbour, float radius)
    {
        //////////////// IF RETURN NEGATİVE ERROR
        float statcons = FindConstant(radius);
        float q = FindDistance(particle, neighbour) / radius;
        float gradient = 0;
        if( 0 <= q && q < 1)
        {
            gradient = (-2 * q) + (3 * q * q / 2);


        }else if ( 1 <= q && q < 2 )
        {
            gradient = (-1 / 2) * (float)Math.Pow((2 - q), 2);
        }
        else
        {
            gradient = 0;
        }
        gradient *= statcons;
        Vector3 ret = ((particle - neighbour) * gradient)/ (FindDistance(particle, neighbour) * radius);
        return ret;
    }

    public float FintLaplacianWeight(Vector3 particle, Vector3 neighbour, float radius)
    {
        float statcons = FindConstant(radius);
        float q = FindDistance(particle, neighbour) / radius;
        float laplacian = 0;
        float laplacianpow = 0;
        if (0 <= q && q < 1)
        {
            laplacian = (-2 * q) + (3 * q * q / 2);
            laplacianpow = -2 + (3 * q);

        }
        else if (1 <= q && q < 2)
        {
            laplacian = (-1 / 2) * (float)Math.Pow((2 - q), 2);
            laplacianpow = 2 - q;
        }
        else
        {
            laplacian = 0;
            laplacianpow = 0;
        }
        laplacian *= statcons;
        laplacianpow *= statcons;

        return (1 / radius * radius) * laplacianpow + (2 / radius) * laplacian; 


    }
    public void CallGradientKernel(Vector3 particle, Vector3 neighbour, float radius)
    {


    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
