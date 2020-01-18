using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SurfaceRecognition
{
    // Start is called before the first frame update
    void Start()
    {

    }

    public Vector4[] _particles;
    public vertexSystem.vertexIndex[] _groups;
    Bounds _bounds;
    float _radius;
    public void SetData(Vector4[] particles, Bounds bounds, ref vertexSystem.vertexIndex[] groups, float radius)
    {
        this._groups = groups;
        this._particles = particles;
        this._bounds = bounds;
        this._radius = radius;
    }

    public float FindDistance(Vector3 vertex, Vector3 point)
    {
        return (float)Math.Sqrt(Math.Pow((vertex.x - point.x), 2) + Math.Pow((vertex.y - point.y), 2) + Math.Pow((vertex.z - point.z), 2));
    }

    public float FindConstant(float radius)
    {
        float cons = 8 / (float)(Math.PI);
        return cons;
    }
    public float FindGradientWeight(Vector3 particle, Vector3 neighbour, float radius)
    {
        //////////////// IF RETURN NEGATİVE ERROR
        float statcons = FindConstant(radius);
        float q = FindDistance(particle, neighbour) / radius;
        float gradient = 0;
        if (0 <= q && q < 0.5)
        {
            gradient = (-2 * q) + (3 * q * q / 2);


        } else if (0.5 <= q && q <= 1)
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
    public void FindNeighParticles(int particleId)
    {
        int[] areaParticles = findBoundary(particleId);
        int[] neighbours = Enumerable.Repeat(-1, 64).ToArray();
        int k = 0;
        for (int i = 0; i < areaParticles.Length; i++)
        {
            for (int j = 0; j < _groups[areaParticles[i]].pointIndice.Length; j++)
            {
                if (_groups[areaParticles[i]].pointIndice[j] != -1)
                {
                    neighbours[k] = _groups[areaParticles[i]].pointIndice[j];
                    k++;
                }
            }
        }
        FindSurfaceParticle(particleId, neighbours);
    }
    public void FindSurfaceParticle(int particleId, int[] neighParticles)
    {

    }
    /*public float FintLaplacianWeight(Vector3 particle, Vector3 neighbour, float radius)
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


    }*/

    public int[] findBoundary(int particleIndice)
    {
        float xMax = _particles[particleIndice].x + _radius * 5;
        if (xMax > _bounds.max.x)
        {
            xMax = _bounds.max.x;
        }

        float xMin = _particles[particleIndice].x - _radius * 5;
        if (xMin < _bounds.min.x)
        {
            xMin = _bounds.min.x;
        }

        float yMax = _particles[particleIndice].y + _radius * 5;
        if (yMax > _bounds.max.y)
        {
            yMax = _bounds.max.y;
        }

        float yMin = _particles[particleIndice].y - _radius * 5;
        if (yMin < _bounds.min.y)
        {
            yMin = _bounds.min.y;
        }

        float zMax = _particles[particleIndice].z + _radius * 5;
        if (zMax > _bounds.max.z)
        {
            zMax = _bounds.max.z;
        }

        float zMin = _particles[particleIndice].z - _radius * 5;
        if (zMin < _bounds.min.z)
        {
            zMin = _bounds.min.z;
        }

        Bounds insideCell = new Bounds();
        insideCell.SetMinMax(new Vector3(xMin, yMin, zMin), new Vector3(xMax, yMax, zMax));
        int[] a = FindAreaCells(insideCell);

        return a;
    }

    public int[] FindAreaCells(Bounds insideCell)
    {
        int _intervalx = (int)Math.Ceiling((_bounds.max.x - _bounds.min.x) / (_radius * 4)); // x ekseninde kaç küçük küp var hesapla.
        int _intervalz = (int)Math.Ceiling((_bounds.max.z - _bounds.min.z) / (_radius * 4));
        int _intervaly = (int)Math.Ceiling((_bounds.max.y - _bounds.min.y) / (_radius * 4));
        int topLeftBackward = FindID(new Vector3(insideCell.min.x, insideCell.max.y, insideCell.min.z));
        int topLeftForward = FindID(new Vector3(insideCell.min.x, insideCell.max.y, insideCell.max.z));
        int topRightBackward = FindID(new Vector3(insideCell.max.x, insideCell.max.y, insideCell.min.z));
        int bottomLeftBackward = FindID(new Vector3(insideCell.min.x, insideCell.min.y, insideCell.min.z));

        int tx = (topRightBackward - topLeftBackward) + 1,
            ty = ((bottomLeftBackward - topLeftBackward) / _intervalx) + 1,
            tz = ((topLeftForward - topLeftBackward) / (_intervalx * _intervaly)) + 1;

        int[] areaNums = Enumerable.Repeat(-1, (9 + ((ty * tx * tz)))).ToArray();
        int i = 0, tempNum = topLeftBackward;

        for (int k = 0; k < ty; k++)
        {
            for (int j = 0; j <= tz; j++)
            {
                for (int m = 0; m < tx; m++)
                {

                    areaNums[i] = tempNum + m;
                    //Debug.Log("aslkdjaşlksdjsaşlkdj"+_groups.Length);
                    //Debug.Log(areaNums[i]);
                    i++;
                }
                tempNum += (_intervalx * _intervaly);
            }
            tempNum = areaNums[0] + (_intervalx * (k + 1));
        }
        //Debug.Log("i is ->>>>" + i + "size is ---->" + areaNums.Length);
        return areaNums;
    }
    public int FindID(Vector3 insideCell)
    {
        int cubeID;
        int _intervalx = (int)Math.Ceiling((_bounds.max.x - _bounds.min.x) / (_radius * 4))-1;
        int _intervaly = (int)Math.Ceiling((_bounds.max.y - _bounds.min.y) / (_radius * 4))-1;

        int xId = (int)Math.Ceiling((insideCell.x - _bounds.min.x) / (_radius * 4));
        int yId = (int)Math.Ceiling((_bounds.max.y - insideCell.y) / (_radius * 4)) ;
        int zId = (int)Math.Ceiling((insideCell.z - _bounds.min.z) / (_radius * 4)) ;
        if (xId == 0 || yId == 0 || zId == 0)
        {
            Debug.Log("under zero");
        }
        cubeID = (xId--) + (_intervalx * (yId--)) + (_intervalx * _intervaly * (zId--));
    
        return cubeID;
    }
    public void retSurfParticles()
    {

    }
    public void CallGradientKernel(Vector3 particle, Vector3 neighbour, float radius)
    {


    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
