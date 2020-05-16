using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SituationalSurfaceCalculator
{
    public float findKernel(float s)
    {
        float q = s * s;

        q = (float)Math.Pow((1 - q), 3);

        return Math.Max(0, q);
    }

    public float FindDistance(Vector3 vertex, Vector3 point)
    {
        return (float)Math.Sqrt(Math.Pow((vertex.x - point.x), 2) + Math.Pow((vertex.y - point.y), 2) + Math.Pow((vertex.z - point.z), 2));
    }


    public Vector3 FindWeightedVector(Vector3 centerParticle, Vector3[] neighbourParticles, float searchRadius)
    {
        Vector3 weightedPosition = new Vector3(0, 0, 0);
        float weightedConstant = 0;

        for (int i = 0; i < neighbourParticles.Length; i++)
        {
            float distance = FindDistance(centerParticle, neighbourParticles[i]);

            weightedPosition += neighbourParticles[i] * findKernel(distance / searchRadius);

            weightedConstant += findKernel(distance / searchRadius);
        }


        return weightedPosition / weightedConstant;
    }


    ////////////////// Zhu and Bridson Part //////////////////
    public float[] findZhuWeights(Vector3 vertex, Vector3[] particles, float[] weights, float length)
    {
        float sum = 0;
        for (int j = 0; j < particles.Length; j++)
        {
            sum += findKernel(FindDistance(vertex, particles[j]) / length);
        }
        for (int i = 0; i < particles.Length; i++)
        {
            weights[i] = findKernel(FindDistance(vertex, particles[i]) / length) / sum;
        }
        return weights;
    }

    public Vector3 weightedPos(Vector3[] particles, float[] weights)
    {
        Vector3 ret = new Vector3(0, 0, 0);
        for (int i = 0; i < particles.Length; i++)
        {
            ret += particles[i] * weights[i];
        }
        return ret;
    }



    public float zhuAndBridson(Vector3 vertex, Vector3 weighted, float radius)
    {
        float xDimension = vertex.x - weighted.x;
        float yDimension = vertex.y - weighted.y;
        float zDimension = vertex.z - weighted.z;

        xDimension = (float)Math.Pow(xDimension, 2);
        yDimension = (float)Math.Pow(yDimension, 2);
        zDimension = (float)Math.Pow(zDimension, 2);

        return (float)Math.Sqrt((xDimension + yDimension + zDimension)) - radius;



    }

}
