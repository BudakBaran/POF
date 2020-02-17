using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Mynose : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        System.Random rnd = new System.Random();
        


        for (int i=0;i<15 ;i++) {

            for (int j = 0; j < 15; j++)
            {
                for (int k = 0; k < 15; k++)
                {
                    int var = rnd.Next(1, 3);

                    if (var == 1)
                    {
                        Gizmos.color = new Color(1, 0, 0, 0.5f);
                        Gizmos.DrawCube(transform.position, new Vector3(1, 1, 1));

                    }
                }

            }
        }
    }



}
