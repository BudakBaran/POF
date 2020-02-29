using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public float findKernel(float s)
    {
        return Math.Max(0, (float)Math.Pow(1 - Math.Pow(s, 2), 3));
    }

    public float findDistance(Vector3 vertex, Vector3 point)
    {
        return (float)Math.Sqrt(Math.Pow((vertex.x - point.x), 2) + Math.Pow((vertex.y - point.y), 2) + Math.Pow((vertex.z - point.z), 2));
    }
    public float[] findWeights(Vector3 vertex, Vector3[] particles, float[] weights, float length) 
    {
        float sum = 0;
        for (int j = 0; j < particles.Length; j++)
        {
            sum += findKernel(findDistance(vertex, particles[j]) / length);
        }
        for (int i = 0; i < particles.Length; i++)
        {
            weights[i] = findKernel(findDistance(vertex, particles[i]) / length) /  sum;
        }
        return weights;
    }

    public Vector3 weightedPos( Vector3[] particles, float[] weights)
    {
        Vector3 ret = new Vector3(0,0,0);
        for ( int i = 0; i < particles.Length; i++)
        {
            ret += particles[i] * weights[i];
        }
        return ret;
    }

    public float ZhuAndBridson(Vector3 vertex, Vector3 weighted, float radius)
    {
        return (float)Math.Sqrt(Math.Pow((vertex.x - weighted.x), 2) + Math.Pow((vertex.y - weighted.y), 2) + Math.Pow((vertex.z - weighted.z), 2)) - radius;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
