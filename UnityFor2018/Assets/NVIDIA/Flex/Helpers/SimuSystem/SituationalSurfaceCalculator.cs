using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SituationalSurfaceCalculator
{

    public Vector4[] particles;
    public vertexSystem.vertexIndex[] groups;
    Bounds bounds;
    Bounds particleNeighbound;

    //////////////////////////////// General FunctionS   ////////////////////////////////////////
    /*
        public float findDistance(Vector3 vertex, Vector3 point)
        {
            return (float)Math.Sqrt(Math.Pow((vertex.x - point.x), 2) + Math.Pow((vertex.y - point.y), 2) + Math.Pow((vertex.z - point.z), 2));
        }
    */


    public float findDistance(Vector3 vertex, Vector3 point)
    {
        int xDimension = vertex.x - point.x;
        int yDimension = vertex.y - point.y;
        int zDimension = vertex.z - point.z;

        xDimension = Math.Pow(xDimension, 2);
        yDimension = Math.Pow(yDimension, 2);
        zDimension = Math.Pow(zDimension, 2);


        return (float)Math.Sqrt(xDimension + yDimension + zDimension);
    }

    //////////////////////////////// SURFACE FINDING PART   ////////////////////////////////////////


    public float findConstant(float radius)
    {
        float cons = 8 / (float)(Math.PI);
        return cons;
    }

    public float findGradientWeight(Vector3 particle, Vector3 neighbour, float radius)
    {
        //////////////// IF RETURN NEGATİVE ERROR
        float statcons = findConstant(radius);
        float q = findDistance(particle, neighbour) / radius;
        float gradient = 0;
        if (0 <= q && q < 0.5)
        {
            gradient = (-2 * q) + (3 * q * q / 2);
        }
        else if (0.5 <= q && q <= 1)
        {
            gradient = (-1 / 2) * (float)Math.Pow((2 - q), 2);
        }
        else if (q > 1)
        {
            gradient = 0;
        }
        gradient *= (statcons / (float)Math.Pow(radius, 3));
        return gradient;
    }


    public List<int> findNeigbourParticles(int[] neighbourCells)
    {
        List<int> neighbours = new List<int>();
        for (int i = 0; i < neighbourCells.Length; i++)
        {
            if (groups[neighbourCells[i]] != null)
            {
                for (int j = 0; j < groups[neighbourCells[i]].pointIndice.Length; j++)
                {
                    if (groups[neighbourCells[i]].pointIndice[j] != -1)
                    {
                        neighbours.Add(groups[neighbourCells[i]].pointIndice[j]);
                    }
                }
            }
        }
        return neighbours;
    }

    public int[] findSurfaceParticles(int particleId, int[] neighbours)
    {
        return null;

    }

    //////////////////////////////// Zhu & Bridson Part   /////////////////////////////////////////////////////////////////


    public float findKernel(float s)
    {
        return Math.Max(0, (float)Math.Pow(1 - Math.Pow(s, 2), 3));
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
            weights[i] = findKernel(findDistance(vertex, particles[i]) / length) / sum;
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



    /*
    public float ZhuAndBridson(Vector3 vertex, Vector3 weighted, float radius)
    {
        return (float)Math.Sqrt(Math.Pow((vertex.x - weighted.x), 2) + Math.Pow((vertex.y - weighted.y), 2) + Math.Pow((vertex.z - weighted.z), 2)) - radius;
    }
*/

    public float zhuAndBridson(Vector3 vertex, Vector3 weighted, float radius)
    {
        int xDimension = vertex.x - weighted.x;
        int yDimension = vertex.y - weighted.y;
        int zDimension = vertex.z - weighted.z;

        xDimension = Math.Pow(xDimension, 2);
        yDimension = Math.Pow(yDimension, 2);
        zDimension = Math.Pow(zDimension, 2);

        return (float)Math.Sqrt((xDimension + yDimension + zDimension)) - radius;



    }

}
