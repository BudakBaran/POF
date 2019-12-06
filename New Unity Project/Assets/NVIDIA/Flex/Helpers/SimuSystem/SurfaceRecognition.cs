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

    public int[] findAreaCells(int particleIndice)
    {
        float xMax = _particles[particleIndice].x + _radius * 4;
        if (xMax > _bounds.max.x)
        {
            xMax = _bounds.max.x;
        }

        float xMin = _particles[particleIndice].x - _radius * 4;
        if (xMin < _bounds.min.x)
        {
            xMin = _bounds.min.x;
        }
            
        float yMax = _particles[particleIndice].y + _radius * 4;
        if (yMax > _bounds.max.y)
        {
            yMax = _bounds.max.y;
        }
            
        float yMin = _particles[particleIndice].y - _radius * 4;
        if (yMin < _bounds.min.y)
        {
            yMin = _bounds.min.y;
        }
            
        float zMax = _particles[particleIndice].z + _radius * 4;
        if (zMax > _bounds.max.z)
        {
            zMax = _bounds.max.z;
        }
            
        float zMin = _particles[particleIndice].z - _radius * 4;
        if (zMin < _bounds.min.z)
        {
            zMin = _bounds.max.z;
        }
            
        Bounds insideCell = new Bounds();
        insideCell.SetMinMax(new Vector3(xMin, yMin, zMin), new Vector3(xMax, yMax, zMax));
        int topLeft = FindID(insideCell.min);
        int bottomRight = FindID(insideCell.max);
        int topRight = FindID(new Vector3(insideCell.min.x, insideCell.max.y, insideCell.min.z));
        //Debug.Log(insideCell.min);
        //Debug.Log("------"+ topLeft + "-----"+ bottomRight + "-----"+ topRight);
        return new int[3] { topLeft, bottomRight, topRight };
    }
    
    public int FindID(Vector3 insideCell)
    {
        int cubeID;
        int _intervalx = (int)Math.Ceiling((_bounds.max.x - _bounds.min.x) / (_radius * 4)); // x ekseninde kaç küçük küp var hesapla.
        int _intervaly = (int)Math.Ceiling((_bounds.max.y - _bounds.min.y) / (_radius * 4));// y ekseninde kaç küçük küp var hesapla.

        int xId = (int)Math.Ceiling((insideCell.x - _bounds.min.x) / (_radius * 4));
        int yId = (int)Math.Ceiling((_bounds.max.y - insideCell.y) / (_radius * 4));
        int zId = (int)Math.Ceiling((insideCell.z - _bounds.min.z) / (_radius * 4));

        // on grid here (x + a(r/8)  === particle.x in some a that occurs so we have to substract 2 value and divide grid size get %)
        cubeID = (xId) + (_intervalx * (yId - 1)) + (_intervalx * _intervaly * (zId - 1));
        //Debug.Log("cube id is ----->" +cubeID);
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
