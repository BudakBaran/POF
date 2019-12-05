using NVIDIA.Flex;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class vertexSystem
{
    public struct vertexIndex
    {
        public int[] pointIndice;
    }

    private vertexIndex[] _vertices ;
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
            findID(_particles[i], (_radius * 4), i);
        }
    }

    public void SetData(int[] indices, Vector4[] particles, Bounds bounds, float radius,ref vertexIndex[] groups)
    {
        // it comes from simuData class
        _indices = indices;
        _particles = particles;
        _bounds = bounds;
        _radius = radius;
        
        this._intervalx = (int)Math.Ceiling((_bounds.max.x - _bounds.min.x) / (_radius * 4)); // x ekseninde kaç küçük küp var hesapla.
        this._intervaly = (int)Math.Ceiling((_bounds.max.y - _bounds.min.y) / (_radius * 4));// y ekseninde kaç küçük küp var hesapla.
        this._intervalz = (int)Math.Ceiling((_bounds.max.z - _bounds.min.z) / (_radius * 4));
        groups = new vertexIndex[this._intervalx * this._intervaly * this._intervalz];
        this._vertices = groups;
    }

    public void checkS(int vertice, int particleIndex)
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
    public void ifExist(int vertex, int point)
    {
        // if found returns index on list, not found returns -1
        int  i= 0, check = 0;
        while (this._vertices[vertex].pointIndice[i] != -1 && i < 63)
        {
            
            if (this._vertices[vertex].pointIndice[i] == point)
                check = 1;
            i++;
        }
        if(check != 1)
        {
            this._vertices[vertex].pointIndice[i] = point;
        }
    }


    public void ifDoesNotExist(int vertice, int particleIndex)
    {
        // create a object fill it and send
        this._vertices[vertice].pointIndice = Enumerable.Repeat(-1, 64).ToArray();
        this._vertices[vertice].pointIndice[0] = particleIndex;
    }
 
    void findID(Vector4 particle, float _length, int _indice) // x,y,z isimlerinde bir particle position datamız var.
    {
        int cubeID;
        
        int xId = (int)Math.Ceiling((particle.x - _bounds.min.x) / _length);
        int yId = (int)Math.Ceiling((_bounds.max.y - particle.y) / _length);
        int zId = (int)Math.Ceiling((particle.z - _bounds.min.z) / _length);

        // on grid here (x + a(r/8)  === particle.x in some a that occurs so we have to substract 2 value and divide grid size get %)
        if ((particle.x - _bounds.min.x) % _radius == 0) {
            cubeID = (xId--) + (this._intervalx * yId) + (this._intervalx * this._intervaly * zId);
          
            checkS(cubeID, _indice);
        }
        if ((_bounds.max.y - particle.y) % _radius == 0) {
            cubeID = (xId) + (this._intervalx * (yId--)) + (this._intervalx * this._intervaly * zId);
 
            checkS(cubeID, _indice);
        }
        if ((particle.z - _bounds.min.x) % _radius == 0) {
            cubeID = (xId) + (this._intervalx * (yId)) + (this._intervalx * this._intervaly * (zId--));
            checkS(cubeID, _indice);
        }
        cubeID = (xId) + (this._intervalx * (yId - 1)) + (this._intervalx * this._intervaly * (zId - 1));
       
        checkS(cubeID-1, _indice);

    }

    SurfaceRecognition a = new SurfaceRecognition();
}
