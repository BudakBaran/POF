using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleFinder
{
    // Start is called before the first frame update
   /* public void FindDimentionalIntervalNum(Bounds bounds, float radius)
    {
        this._intervalx = (int)Math.Ceiling((_bounds.max.x - _bounds.min.x) / _radius); // x ekseninde kaç küçük küp var hesapla.
        this._intervaly = (int)Math.Ceiling((_bounds.max.y - _bounds.min.y) / _radius); // y ekseninde kaç küçük küp var hesapla.
        this._intervalz = (int)Math.Ceiling((_bounds.max.z - _bounds.min.z) / _radius);
        // returns int array[3]
    }

    public void FindId(Vector3 objectX, Bounds bounds)
    {
        int xId = (int)Math.Ceiling((particle.x - _bounds.min.x) / _length) - 1;
        int yId = (int)Math.Ceiling((_bounds.max.y - particle.y) / _length) - 1;
        int zId = (int)Math.Ceiling((particle.z - _bounds.min.z) / _length) - 1;
        //returns int array[3]
    }
    // Update is called once per frame
    public Bounds findBoundary(int particleIndice, float radius, int Distance) // Particle finder'a koy.
    {
        float xMax = _particles[particleIndice].x + radius * Distance;
        if (xMax > _bounds.max.x)
        {
            xMax = _bounds.max.x;
        }

        float xMin = _particles[particleIndice].x - radius * Distance;
        if (xMin < _bounds.min.x)
        {
            xMin = _bounds.min.x;
        }

        float yMax = _particles[particleIndice].y + radius * Distance;
        if (yMax > _bounds.max.y)
        {
            yMax = _bounds.max.y;
        }

        float yMin = _particles[particleIndice].y - radius * Distance;
        if (yMin < _bounds.min.y)
        {
            yMin = _bounds.min.y;
        }

        float zMax = _particles[particleIndice].z + radius * Distance;
        if (zMax > _bounds.max.z)
        {
            zMax = _bounds.max.z;
        }

        float zMin = _particles[particleIndice].z - radius * Distance;
        if (zMin < _bounds.min.z)
        {
            zMin = _bounds.min.z;
        }

        Bounds insideCell = new Bounds();
        insideCell.SetMinMax(new Vector3(xMin, yMin, zMin), new Vector3(xMax, yMax, zMax));
        return insideCell;
    }*/
}

