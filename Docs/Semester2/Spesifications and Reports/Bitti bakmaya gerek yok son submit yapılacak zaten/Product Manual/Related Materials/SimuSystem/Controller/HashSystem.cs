using NVIDIA.Flex;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class HashSystem
{
    public class HashModel
    {
        public int[] pointIndice = new int[1] { -1 };
    }

    private HashModel[] _vertices;
    // Used for mathematical calculations and setting datas to vertex point
    // always get data change data
    // (find a vertex data from 3d surface)


    public int[] _indices;
    int[] intervals;
    public Vector4[] _particles;
    Bounds _bounds;
    float _radius;
    public int z = 10;
    public ParticleFinder _ParticleFinder = new ParticleFinder();

    // Vertex system runs vertexPoint and get data
    public void GroupByCells()
    {
        // setting of hash Indıce is vertex num Insıde array is adress in list
        for (int i = 0; i < _particles.Length; i++)
        {
            findID(_particles[i], _radius, i);
        }
    }

    public void SetData(int[] indices, Vector4[] particles, Bounds bounds, float radius, ref HashModel[] groups)
    {
        // it comes from simuData class
        _indices = indices;
        _particles = particles;
        _bounds = bounds;
        _radius = radius;

        this.intervals = _ParticleFinder.FindDimentionalIntervalNum(bounds, radius); // 0 -> x    1 -> y      2 -> z

        groups = new HashModel[this.intervals[0] * this.intervals[1] * this.intervals[2]]; // decide hash size
        this._vertices = groups;
    }

    public void checkS(int vertice, int particleIndex)
    {
        if(this._vertices[vertice] != null)
        {
            if (this._vertices[vertice].pointIndice == null)
            {

                ifDoesNotExist(vertice, particleIndex);
            }
            else
            {

                ifExist(vertice, particleIndex);
            }
        }
        else
        {
            this._vertices[vertice] = new HashModel();
            ifDoesNotExist(vertice, particleIndex);
        }

        
    }
    public void ifExist(int vertex, int point)
    {
        // if found returns index on list, not found returns -1
        int i = 0, check = 0;
        while (this._vertices[vertex].pointIndice[i] != -1 && i < 8)
        {

            if (this._vertices[vertex].pointIndice[i] == point)
                check = 1;
            i++;
        }
        if (check != 1)
        {
            this._vertices[vertex].pointIndice[i] = point;
        }
    }

    public void ifDoesNotExist(int vertice, int particleIndex)
    {
        // create a object fill it and send
        this._vertices[vertice].pointIndice = Enumerable.Repeat(-1, 8).ToArray();
        this._vertices[vertice].pointIndice[0] = particleIndex;
    }


    // Find particle in which cells
    void findID(Vector4 particle, float _length, int _indice)
    {
        int cubeID;
        // 0 - 1 // 1 - 2 // 2 - 3

        int[] XYZIntervalID = _ParticleFinder.FindId(particle, _bounds, _length);
        XYZIntervalID[0] -= 1;
        XYZIntervalID[1] -= 1;
        XYZIntervalID[2] -= 1;
       
        float cubeX = (particle.x - _bounds.min.x) % _radius; // With % we can assume is this particle member of the two hash cell
        float cubeY = (_bounds.max.y - particle.y) % _radius;
        float cubeZ = (particle.z - _bounds.min.z) % _radius;

        // Five errors in here for fill to fix
        if (cubeX == 0 && cubeY == 0 && cubeZ == 0)
        {
            if (XYZIntervalID[0] > 0 && XYZIntervalID[1] > 0 && XYZIntervalID[2] > 0)  // -> Cells that close to boundary will show error so we check this and ceiling or rounding
            {
                cubeID = (XYZIntervalID[0]--) + (this.intervals[0] * (XYZIntervalID[1]--)) + (this.intervals[0] * this.intervals[1] * (XYZIntervalID[2]--));
                checkS(cubeID, _indice);
            }
            if (XYZIntervalID[0] > 0 && XYZIntervalID[1] > 0)
            {
                cubeID = (XYZIntervalID[0]--) + (this.intervals[0] * (XYZIntervalID[1]--)) + (this.intervals[0] * this.intervals[1] * (XYZIntervalID[2]));
                checkS(cubeID, _indice);
            }
            if (XYZIntervalID[0] > 0 && XYZIntervalID[2] > 0)
            {
                cubeID = (XYZIntervalID[0]--) + (this.intervals[0] * (XYZIntervalID[1])) + (this.intervals[0] * this.intervals[1] * (XYZIntervalID[2]--));
                checkS(cubeID, _indice);
            }
            if (XYZIntervalID[0] > 0)
            {
                cubeID = (XYZIntervalID[0]--) + (this.intervals[0] * (XYZIntervalID[1])) + (this.intervals[0] * this.intervals[1] * (XYZIntervalID[2]));
                checkS(cubeID, _indice);
            }

            if (XYZIntervalID[1] > 0 && XYZIntervalID[2] > 0)
            {
                cubeID = (XYZIntervalID[0]) + (this.intervals[0] * (XYZIntervalID[1]--)) + (this.intervals[0] * this.intervals[1] * (XYZIntervalID[2]--));
                checkS(cubeID, _indice);
            }

            if (XYZIntervalID[1] > 0)
            {
                cubeID = (XYZIntervalID[0]) + (this.intervals[0] * (XYZIntervalID[1]--)) + (this.intervals[0] * this.intervals[1] * (XYZIntervalID[2]));
                checkS(cubeID, _indice);
            }

            if (XYZIntervalID[2] > 0)
            {
                cubeID = (XYZIntervalID[0]) + (this.intervals[0] * (XYZIntervalID[1])) + (this.intervals[0] * this.intervals[1] * (XYZIntervalID[2]--));
                checkS(cubeID, _indice);
            }

            cubeID = (XYZIntervalID[0]) + (this.intervals[0] * (XYZIntervalID[1])) + (this.intervals[0] * this.intervals[1] * (XYZIntervalID[2]));
            checkS(cubeID, _indice);
        }
        else if (cubeX == 0 && cubeY == 0)
        {
            if (XYZIntervalID[1] > 0 && XYZIntervalID[0] > 0)
            {
                cubeID = (XYZIntervalID[0]--) + (this.intervals[0] * (XYZIntervalID[1]--)) + (this.intervals[0] * this.intervals[1] * (XYZIntervalID[2]));
                checkS(cubeID, _indice);
            }
            if (XYZIntervalID[0] > 0)
            {
                cubeID = (XYZIntervalID[0]--) + (this.intervals[0] * (XYZIntervalID[1])) + (this.intervals[0] * this.intervals[1] * (XYZIntervalID[2]));
                checkS(cubeID, _indice);
            }

            if (XYZIntervalID[1] > 0)
            {
                cubeID = (XYZIntervalID[0]) + (this.intervals[0] * (XYZIntervalID[1]--)) + (this.intervals[0] * this.intervals[1] * (XYZIntervalID[2]));
                checkS(cubeID, _indice);
            }
            // X is +

            cubeID = (XYZIntervalID[0]) + (this.intervals[0] * (XYZIntervalID[1])) + (this.intervals[0] * this.intervals[1] * (XYZIntervalID[2]));
            checkS(cubeID, _indice);

        }
        else if (cubeX == 0 && cubeZ == 0)
        {
            if (XYZIntervalID[0] > 0 && XYZIntervalID[2] > 0)
            {
                cubeID = (XYZIntervalID[0]--) + (this.intervals[0] * (XYZIntervalID[1])) + (this.intervals[0] * this.intervals[1] * (XYZIntervalID[2])--);
                checkS(cubeID, _indice);
            }
            if (XYZIntervalID[0] > 0)
            {
                cubeID = (XYZIntervalID[0]--) + (this.intervals[0] * (XYZIntervalID[1])) + (this.intervals[0] * this.intervals[1] * (XYZIntervalID[2]));
                checkS(cubeID, _indice);
            }
            if (XYZIntervalID[2] > 0)
            {
                cubeID = (XYZIntervalID[0]) + (this.intervals[0] * (XYZIntervalID[1])) + (this.intervals[0] * this.intervals[1] * (XYZIntervalID[2]--));
                checkS(cubeID, _indice);
            }

            cubeID = (XYZIntervalID[0]) + (this.intervals[0] * (XYZIntervalID[1])) + (this.intervals[0] * this.intervals[1] * (XYZIntervalID[2]));
            checkS(cubeID, _indice);
        }
        else if (cubeY == 0 && cubeZ == 0)
        {
            if (XYZIntervalID[1] > 0 && XYZIntervalID[2] > 0)
            {
                cubeID = (XYZIntervalID[0]) + (this.intervals[0] * (XYZIntervalID[1]--)) + (this.intervals[0] * this.intervals[1] * (XYZIntervalID[2]--));
                checkS(cubeID, _indice);
            }
            if (XYZIntervalID[1] > 0)
            {
                cubeID = (XYZIntervalID[0]) + (this.intervals[0] * (XYZIntervalID[1]--)) + (this.intervals[0] * this.intervals[1] * (XYZIntervalID[2]));
                checkS(cubeID, _indice);
            }
            if (XYZIntervalID[2] > 0)
            {
                cubeID = (XYZIntervalID[0]) + (this.intervals[0] * (XYZIntervalID[1])) + (this.intervals[0] * this.intervals[1] * (XYZIntervalID[2]--));
                checkS(cubeID, _indice);
            }
            // Y is +

            cubeID = (XYZIntervalID[0]) + (this.intervals[0] * (XYZIntervalID[1])) + (this.intervals[0] * this.intervals[1] * (XYZIntervalID[2]));
            checkS(cubeID, _indice);
        }
        else
        {
            cubeID = (XYZIntervalID[0]) + (this.intervals[0] * (XYZIntervalID[1])) + (this.intervals[0] * this.intervals[1] * (XYZIntervalID[2]));
            checkS(cubeID, _indice);
        }
    }
}
