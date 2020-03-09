using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Change this class name as SurfaceParticleFinder.

public class SurfaceRecognition
{
    // Start is called before the first frame update
    void Start()
    {

    }

    public Vector4[] _particles;
    public HashSystem.HashModel[] _groups;
    Bounds _bounds;
    Bounds _ParticleNeighbound;

    float _radius;
    public void SetData(Vector4[] particles, Bounds bounds, ref HashSystem.HashModel[] groups, float radius)
    {
        this._groups = groups;
        this._particles = particles;
        this._bounds = bounds;
        this._radius = radius;
    }

    public float findKernel(float s)
    {
        float q = s;

        q = (float)Math.Pow(1 - Math.Pow(q, 2), 3);

        return Math.Max(0, q);
    }

    public float FindDistance(Vector3 vertex, Vector3 point)
    {
        return (float)Math.Sqrt(Math.Pow((vertex.x - point.x), 2) + Math.Pow((vertex.y - point.y), 2) + Math.Pow((vertex.z - point.z), 2));
    }

    public List<Vector3> FindNeigbourParticles(int [] neighbourCells)
    {
        List<Vector3> neighbours = new List<Vector3>();
        for (int i = 0; i < neighbourCells.Length; i++)
        {
            if (neighbourCells[i] != -1) { 
                if (_groups[neighbourCells[i]] != null)
                {
                    for (int j = 0; j < _groups[neighbourCells[i]].pointIndice.Length; j++)
                    {
                        if (_groups[neighbourCells[i]].pointIndice[j] != -1)
                        {
                            float x = _particles[_groups[neighbourCells[i]].pointIndice[j]].x;
                            float y = _particles[_groups[neighbourCells[i]].pointIndice[j]].y;
                            float z = _particles[_groups[neighbourCells[i]].pointIndice[j]].z;
                            neighbours.Add(new Vector3(x, y, z));
                        }
                    }
                }
            }
        }
        return neighbours;
    }


    public Vector3 FindWeigtedX(Vector3 centerParticle, Vector3[] neighbourParticles, float searchRadius)
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

    public bool isSurfaceParticle(Vector3 centerParticle, Vector3[] neighbourParticles)
    {
        float returnVal = FindDistance(centerParticle, FindWeigtedX(centerParticle, neighbourParticles, _radius * 8));
        if (returnVal < 0.102826)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    public int[] findBoundary() // Particle finder'a koy.
    {
        Bounds insideCell = new Bounds();
        int[] neigbourCells = { -1 };
        List<int> surfaceParticles = new List<int>();

        for (int i = 0; i < 15526; i++)
        {
            float xMax = _particles[i].x + _radius * 8;
            if (xMax > _bounds.max.x)
            {
                xMax = _bounds.max.x;
            }

            float xMin = _particles[i].x - _radius * 8;
            if (xMin < _bounds.min.x)
            {
                xMin = _bounds.min.x;
            }

            float yMax = _particles[i].y + _radius * 8;
            if (yMax > _bounds.max.y)
            {
                yMax = _bounds.max.y;
            }

            float yMin = _particles[i].y - _radius * 8;
            if (yMin < _bounds.min.y)
            {
                yMin = _bounds.min.y;
            }

            float zMax = _particles[i].z + _radius * 8;
            if (zMax > _bounds.max.z)
            {
                zMax = _bounds.max.z;
            }

            float zMin = _particles[i].z - _radius * 8;
            if (zMin < _bounds.min.z)
            {
                zMin = _bounds.min.z;
            }

            insideCell.SetMinMax(new Vector3(xMin, yMin, zMin), new Vector3(xMax, yMax, zMax));
            neigbourCells = FindAreaCells(insideCell);
            bool isSurface = isSurfaceParticle(_particles[i], FindNeigbourParticles(neigbourCells).ToArray());
            if (isSurface)
            {
                surfaceParticles.Add(i);
            }
        }
        this._ParticleNeighbound = insideCell;
        return surfaceParticles.ToArray();
        //return neigbourCells;
    }

    //int[],

        //  SETTER AND GETTER
        
        public Bounds GetParticleBound() 
        {
            return _ParticleNeighbound; 
        }
    public int[] FindAreaCells(Bounds insideCell) // Particle finder'a koy.
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
    public int FindID(Vector3 insideCell) // Particle finder'a koy.
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
    public void ReturnSurfParticles()
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
