using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vertexPoint : MonoBehaviour
{
    // Start is called before the first frame update
    public struct vertexIndex
    {
        public Vector3 vertexs;
        public List<int> pointIndice;
    }

    private Vector3 tempVertex;
    private int tempPoint = 0;
    private List<vertexIndex> vertices;
    //private List<vertexPoint> vertices = new List<vertexPoint>();
    // Start is called before the first frame update

    /// <summary>
    /// Every time we need vertices point we use this function
    /// </summary>
    public List<vertexIndex> getVertexPoint()
    {
        return this.vertices;
    }

    /// <summary>
    /// Start with reset
    /// </summary>
    public void resetVertices()
    {
        this.vertices = new List<vertexIndex>();
    }

    /// <summary>
    /// Basic adding for one point
    /// </summary>
    public void setTemp(Vector3 a, int b)
    {
        this.tempVertex = a;
        this.tempPoint = b;

    }

    public void checkS(Vector3 vertice)
    {
        int a = vertices.FindIndex(b => b.vertexs == vertice);
        if (a != -1)
        {
            // Check if point is already added
            ifExist(tempPoint);
        }
        else
        {
            ifDoesNotExist();
        }
    }
    public void ifExist(int point)
    {
        // if found returns index on list, not found returns -1
        int a = vertices.FindIndex(b => b.vertexs == this.tempVertex);
       // Debug.Log(a);
        vertices[a].pointIndice.Add(point);
    }


    public void ifDoesNotExist()
    {
        vertexIndex temp = new vertexIndex();
        temp.pointIndice = new List<int>();
        temp.vertexs = this.tempVertex;
        temp.pointIndice.Add(this.tempPoint);
        this.vertices.Add(temp);
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
