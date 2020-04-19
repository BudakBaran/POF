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
    public ParticleFinder _ParticleFinder = new ParticleFinder();
    public SituationalSurfaceCalculator _SurfaceCalculator = new SituationalSurfaceCalculator();
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

    public bool isSurfaceParticle(Vector3 centerParticle, Vector3[] neighbourParticles)
    {
        Vector3 weightedVector = _SurfaceCalculator.FindWeightedVector(centerParticle, neighbourParticles, _radius * 4);

        float threshold = _SurfaceCalculator.FindDistance(centerParticle, weightedVector);
   
        if (threshold < 0.035)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    // Get surface particles and return it
    public int[] findBoundary()
    {
        int[] neigbourCells = { -1 };
        List<int> surfaceParticles = new List<int>();
        Bounds insideCell = new Bounds();
        for (int i = 0; i < 4096; i++)
        {
            Vector4 sendParticle = _particles[i];
            insideCell = _ParticleFinder.FindNeighbourArea(sendParticle, _radius,4,_bounds);

            neigbourCells = FindAreaCells(insideCell);

            List<Vector3> neighbourParticles = _ParticleFinder.FindNeigbourParticles(ref _groups, ref _particles, neigbourCells);

            bool isSurface = isSurfaceParticle(_particles[i], neighbourParticles.ToArray());

            if (isSurface)
            {
                surfaceParticles.Add(i);
            }
        }
        this._ParticleNeighbound = insideCell;
        return surfaceParticles.ToArray();
    }


    // Find cell numbers from given area
    public int[] FindAreaCells(Bounds insideCell) 
    {
        int[] intervals = _ParticleFinder.FindDimentionalIntervalNum(_bounds, _radius);

        int topLeftBackward = FindID(new Vector3(insideCell.min.x, insideCell.max.y, insideCell.min.z));
        int topLeftForward = FindID(new Vector3(insideCell.min.x, insideCell.max.y, insideCell.max.z));
        int topRightBackward = FindID(new Vector3(insideCell.max.x, insideCell.max.y, insideCell.min.z));
        int bottomLeftBackward = FindID(new Vector3(insideCell.min.x, insideCell.min.y, insideCell.min.z));

        int tx = (topRightBackward - topLeftBackward),
            ty = ((bottomLeftBackward - topLeftBackward) / intervals[0]),
            tz = ((topLeftForward - topLeftBackward) / (intervals[0] * intervals[1]));

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
                tempNum += (intervals[0] * intervals[1]);
            }
            tempNum = areaNums[0] + (intervals[0] * (k + 1));
        }
        return areaNums;
    }

    public int FindID(Vector3 insideCell)
    {
        int cubeID;
        int[] intervals = _ParticleFinder.FindDimentionalIntervalNum(_bounds, _radius);

        int[] dimentionalID = _ParticleFinder.FindId(insideCell, _bounds, _radius);
        cubeID = (dimentionalID[0]) + (intervals[0] * (dimentionalID[1])) + (intervals[0] * intervals[1] * (dimentionalID[2]));
    
        return cubeID;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
