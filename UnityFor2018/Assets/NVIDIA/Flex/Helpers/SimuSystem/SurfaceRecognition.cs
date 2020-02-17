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
    Bounds _ParticleNeighbound;

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

    public List<int> FindNeigbourParticles(int [] neighbourCells)
    {
        List<int> neighbours = new List<int>();
        for (int i = 0; i < neighbourCells.Length; i++)
        {
            if (_groups[neighbourCells[i]] != null)
            {
                for (int j = 0; j < _groups[neighbourCells[i]].pointIndice.Length; j++)
                {
                    if (_groups[neighbourCells[i]].pointIndice[j] != -1)
                    {
                        neighbours.Add(_groups[neighbourCells[i]].pointIndice[j]);
                    }
                }
            }
        }
        return neighbours;
    }

    public int[] FindSurfaceParticles(int particleId, int[] neighbours)
    {
        return null;
        //
    }

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

        int[] neigbourCells = FindAreaCells(insideCell);
        
        this._ParticleNeighbound = insideCell; 

        int[] neighbourParticles = FindNeigbourParticles(neigbourCells).ToArray();
        FindSurfaceParticles(particleIndice, neighbourParticles);
        return neigbourCells;
    }

    //int[],

        //  SETTER AND GETTER
        
        public Bounds GetParticleBound() 
        {
            return _ParticleNeighbound; 
        }
    public int[] FindAreaCells(Bounds insideCell)
    {
        int _intervalx = (int)Math.Ceiling((_bounds.max.x - _bounds.min.x) / _radius);
        int _intervaly = (int)Math.Ceiling((_bounds.max.y - _bounds.min.y) / _radius);
        int topLeftBackward = FindID(new Vector3(insideCell.min.x, insideCell.max.y, insideCell.min.z));
        int topLeftForward = FindID(new Vector3(insideCell.min.x, insideCell.max.y, insideCell.max.z));
        int topRightBackward = FindID(new Vector3(insideCell.max.x, insideCell.max.y, insideCell.min.z));
        int bottomLeftBackward = FindID(new Vector3(insideCell.min.x, insideCell.min.y, insideCell.min.z));

        int tx = (topRightBackward - topLeftBackward),
            ty = ((bottomLeftBackward - topLeftBackward) / _intervalx),
            tz = ((topLeftForward - topLeftBackward) / (_intervalx * _intervaly));

        int[] areaNums = Enumerable.Repeat(-1, ( (ty+1) * (tx+1) * (tz+1)) ).ToArray();
        int i = 0, tempNum = topLeftBackward;

        for (int k = 0; k < ty; k++)
        {
            for (int j = 0; j < tz; j++)
            {
                for (int m = 0; m < tx; m++)
                {
                    areaNums[i] = tempNum + m;
                    i++;
                }
                tempNum += (_intervalx * _intervaly);
            }
            tempNum = areaNums[0] + (_intervalx * (k + 1));
        }
        return areaNums;
    }
    public int FindID(Vector3 insideCell)
    {
        int cubeID;
        int _intervalx = (int)Math.Ceiling((_bounds.max.x - _bounds.min.x) / (_radius));
        int _intervaly = (int)Math.Ceiling((_bounds.max.y - _bounds.min.y) / (_radius));

        int xId = (int)Math.Ceiling((insideCell.x - _bounds.min.x) / (_radius));
        int yId = (int)Math.Ceiling((_bounds.max.y - insideCell.y) / (_radius));
        int zId = (int)Math.Ceiling((insideCell.z - _bounds.min.z) / (_radius));
        cubeID = (xId) + (_intervalx * (yId)) + (_intervalx * _intervaly * (zId));
    
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
