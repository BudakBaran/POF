using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleFinder
{
    // Start is called before the first frame update
    public int[] FindDimentionalIntervalNum(Bounds bounds, float radius)
    {
        int[] intervals = new int[3];
        intervals[0] = (int)Math.Ceiling((bounds.max.x - bounds.min.x) / radius); // x ekseninde kaç küçük küp var hesapla.
        intervals[1] = (int)Math.Ceiling((bounds.max.y - bounds.min.y) / radius); // y ekseninde kaç küçük küp var hesapla.
        intervals[2] = (int)Math.Ceiling((bounds.max.z - bounds.min.z) / radius);

        return intervals;
    }

    public int[] FindId(Vector3 objectX, Bounds bounds, float length)
    {
        int[] dinmentionalID = new int[3];
        dinmentionalID[0] = (int)Math.Ceiling((objectX.x - bounds.min.x) / length);
        dinmentionalID[1] = (int)Math.Ceiling((bounds.max.y - objectX.y) / length);
        dinmentionalID[2] = (int)Math.Ceiling((objectX.z - bounds.min.z) / length);

        return dinmentionalID;
    }
    // Update is called once per frame
    public Bounds FindNeighbourArea(Vector4 particle, float radius, int distance, Bounds bounds) // Particle finder'a koy.
    {
        Bounds insideCell = new Bounds();
        float xMax = particle.x + radius * distance;
        if (xMax > bounds.max.x)
        {
            xMax = bounds.max.x;
        }

        float xMin = particle.x - radius * distance;
        if (xMin < bounds.min.x)
        {
            xMin = bounds.min.x;
        }

        float yMax = particle.y + radius * distance;
        if (yMax > bounds.max.y)
        {
            yMax = bounds.max.y;
        }

        float yMin = particle.y - radius * distance;
        if (yMin < bounds.min.y)
        {
            yMin = bounds.min.y;
        }

        float zMax = particle.z + radius * distance;
        if (zMax > bounds.max.z)
        {
            zMax = bounds.max.z;
        }

        float zMin = particle.z - radius * distance;
        if (zMin < bounds.min.z)
        {
            zMin = bounds.min.z;
        }

        insideCell.SetMinMax(new Vector3(xMin, yMin, zMin), new Vector3(xMax, yMax, zMax));
        return insideCell;
    }

    public List<Vector3> FindNeigbourParticles(ref HashSystem.HashModel[] _groups , ref Vector4[] _particles ,int[] neighbourCells)
    {
        List<Vector3> neighbours = new List<Vector3>();
        for (int i = 0; i < neighbourCells.Length; i++)
        {
            if (neighbourCells[i] != -1)
            {
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


}

