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
        public int vertexIndices;
        public List<int> pointIndice;
    }

    private int tempPoint = 0;
    private List<vertexIndex> vertices;
    // Used for mathematical calculations and setting datas to vertex point
    // always get data change data
    // (find a vertex data from 3d surface)


    public int[] _indices;
    public Vector4[] _particles;
    Bounds _bounds;
    float _radius;
    public int z = 10;

    // Vertex system runs vertexPoint and get data

    public void SetData(int[] indices, Vector4[] particles, Bounds bounds, float radius)
    {
        // it comes from simuData class
        _indices = indices;
        _particles = particles;
        _bounds = bounds;
        _radius = radius;
    }

    public void checkS(int vertice, int particleIndex)
    {
        int a = vertices.FindIndex(b => b.vertexIndices == vertice);
        if (a != -1)
        {
            // Check if point is already added
            ifExist(a, particleIndex);
        }
        else
        {
            ifDoesNotExist(vertice, particleIndex);
        }
    }
    public void ifExist(int vertex, int point)
    {
        // if found returns index on list, not found returns -1
        vertices[vertex].pointIndice.Add(point);
    }


    public void ifDoesNotExist(int vertice, int particleIndex)
    {
        // create a object fill it and send
        vertexIndex temp = new vertexIndex();
        temp.pointIndice = new List<int>();
        temp.vertexIndices = vertice;
        temp.pointIndice.Add(this.tempPoint);
        this.vertices.Add(temp);
    }

    float findID(Vector4 particle) // x,y,z isimlerinde bir particle position datamız var.
    {
        int cubeID;

        // Şekil dikdörtgen prizma olabilir diye her eksendeki küp sayısını ayrı hesapladık.
        //Şekil küp ise tek bir interval değerini hepsine uygula.
        int intervalx = (int)Math.Ceiling((_bounds.max.x - _bounds.min.x) / _radius); // x ekseninde kaç küçük küp var hesapla.
        int intervaly = (int)Math.Ceiling((_bounds.max.y - _bounds.min.y) / _radius); // y ekseninde kaç küçük küp var hesapla.
        /// Why ceiling and what it doo
        /// Çizgi üzeri durumlar var düzelt.
        /// Why everything turkish are we idiot ????
        /// Vector 3D sort research

        int xId = (int)Math.Round((particle.x - _bounds.min.x) / _radius);
        int yId = (int)Math.Round((_bounds.max.y - particle.y) / _radius);
        int zId = (int)Math.Round((particle.z - _bounds.min.x) / _radius);
        //Eğer küp orjinden başlarsa, yani (0,0,0) ise;

        // on grid here (x + a(r/8)  === particle.x in some a that occurs so we have to substract 2 value and divide grid size get %)
        if ((particle.x - _bounds.min.x) % _radius == 0) {
            xId++;
        } else if((_bounds.max.y - particle.y) % _radius == 0){
            yId++;
        } else if((particle.z - _bounds.min.x) % _radius == 0){
            zId++;
        }
        cubeID = (xId + 1) + (intervalx * yId) + (intervalx * intervaly * zId);

        Debug.Log("Cube id is:" + cubeID);

        return cubeID;
    }

    public void Fill()
    {

    }
    public void setVertex(Vector3 vertice)
    {

    }

    public void getVertex()
    {

    }


    // Update is called once per frame
    void Update()
    {

        // do calculations and reset for each frame
        // every step if we change a vertices call get vertices
    }


    internal void setTemp(Vector3 a, int v)
    {
        throw new NotImplementedException();
    }
}
