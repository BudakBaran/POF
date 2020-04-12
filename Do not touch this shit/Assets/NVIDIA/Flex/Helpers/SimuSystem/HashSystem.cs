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
    public int _intervalx;
    public int _intervaly;
    public int _intervalz;
    public Vector4[] _particles;
    Bounds _bounds;
    float _radius;
    public int z = 10;

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

        this._intervalx = (int)Math.Ceiling((_bounds.max.x - _bounds.min.x) / _radius); // x ekseninde kaç küçük küp var hesapla.
        this._intervaly = (int)Math.Ceiling((_bounds.max.y - _bounds.min.y) / _radius); // y ekseninde kaç küçük küp var hesapla.
        this._intervalz = (int)Math.Ceiling((_bounds.max.z - _bounds.min.z) / _radius);

        groups = new HashModel[this._intervalx * this._intervaly * this._intervalz]; // decide hash size
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
        int xId = (int)Math.Ceiling((particle.x - _bounds.min.x) / _length) - 1;
        int yId = (int)Math.Ceiling((_bounds.max.y - particle.y) / _length) - 1;
        int zId = (int)Math.Ceiling((particle.z - _bounds.min.z) / _length) - 1;
        float cubeX = (particle.x - _bounds.min.x) % _radius ;
        float cubeY = (_bounds.max.y - particle.y) % _radius;
        float cubeZ = (particle.z - _bounds.min.z) % _radius;

        // Five errors in here for fill to fix
        if (cubeX == 0 && cubeY == 0 && cubeZ == 0)
        {
            if (xId > 0 && yId > 0 && zId > 0)
            {
                cubeID = (xId--) + (this._intervalx * (yId--)) + (this._intervalx * this._intervaly * (zId--));
                checkS(cubeID, _indice);
            }
            if (xId > 0 && yId > 0)
            {
                cubeID = (xId--) + (this._intervalx * (yId--)) + (this._intervalx * this._intervaly * (zId));
                checkS(cubeID, _indice);
            }
            if (xId > 0 && zId > 0)
            {
                cubeID = (xId--) + (this._intervalx * (yId)) + (this._intervalx * this._intervaly * (zId--));
                checkS(cubeID, _indice);
            }
            if (xId > 0)
            {
                cubeID = (xId--) + (this._intervalx * (yId)) + (this._intervalx * this._intervaly * (zId));
                checkS(cubeID, _indice);
            }

            if (yId > 0 && zId > 0)
            {
                cubeID = (xId) + (this._intervalx * (yId--)) + (this._intervalx * this._intervaly * (zId--));
                checkS(cubeID, _indice);
            }

            if (yId > 0)
            {
                cubeID = (xId) + (this._intervalx * (yId--)) + (this._intervalx * this._intervaly * (zId));
                checkS(cubeID, _indice);
            }

            if (zId > 0)
            {
                cubeID = (xId) + (this._intervalx * (yId)) + (this._intervalx * this._intervaly * (zId--));
                checkS(cubeID, _indice);
            }

            cubeID = (xId) + (this._intervalx * (yId)) + (this._intervalx * this._intervaly * (zId));
            checkS(cubeID, _indice);
        }
        else if (cubeX == 0 && cubeY == 0)
        {
            if (yId > 0 && xId > 0)
            {
                cubeID = (xId--) + (this._intervalx * (yId--)) + (this._intervalx * this._intervaly * (zId));
                checkS(cubeID, _indice);
            }
            if (xId > 0)
            {
                cubeID = (xId--) + (this._intervalx * (yId)) + (this._intervalx * this._intervaly * (zId));
                checkS(cubeID, _indice);
            }

            if (yId > 0)
            {
                cubeID = (xId) + (this._intervalx * (yId--)) + (this._intervalx * this._intervaly * (zId));
                checkS(cubeID, _indice);
            }
            // X is +

            cubeID = (xId) + (this._intervalx * (yId)) + (this._intervalx * this._intervaly * (zId));
            checkS(cubeID, _indice);

        }
        else if (cubeX == 0 && cubeZ == 0)
        {
            if (xId > 0 && zId > 0)
            {
                cubeID = (xId--) + (this._intervalx * (yId)) + (this._intervalx * this._intervaly * (zId)--);
                checkS(cubeID, _indice);
            }
            if (xId > 0)
            {
                cubeID = (xId--) + (this._intervalx * (yId)) + (this._intervalx * this._intervaly * (zId));
                checkS(cubeID, _indice);
            }
            if (zId > 0)
            {
                cubeID = (xId) + (this._intervalx * (yId)) + (this._intervalx * this._intervaly * (zId--));
                checkS(cubeID, _indice);
            }

            cubeID = (xId) + (this._intervalx * (yId)) + (this._intervalx * this._intervaly * (zId));
            checkS(cubeID, _indice);
        }
        else if (cubeY == 0 && cubeZ == 0)
        {
            if (yId > 0 && zId > 0)
            {
                cubeID = (xId) + (this._intervalx * (yId--)) + (this._intervalx * this._intervaly * (zId--));
                checkS(cubeID, _indice);
            }
            if (yId > 0)
            {
                cubeID = (xId) + (this._intervalx * (yId--)) + (this._intervalx * this._intervaly * (zId));
                checkS(cubeID, _indice);
            }
            if (zId > 0)
            {
                cubeID = (xId) + (this._intervalx * (yId)) + (this._intervalx * this._intervaly * (zId--));
                checkS(cubeID, _indice);
            }
            // Y is +

            cubeID = (xId) + (this._intervalx * (yId)) + (this._intervalx * this._intervaly * (zId));
            checkS(cubeID, _indice);
        }
        else
        {
            cubeID = (xId) + (this._intervalx * (yId)) + (this._intervalx * this._intervaly * (zId));
            checkS(cubeID, _indice);
        }
    }
}
