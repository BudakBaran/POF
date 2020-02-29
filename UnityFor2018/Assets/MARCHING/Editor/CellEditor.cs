using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

[CustomEditor(typeof(CellDrawer))] 
//[CustomPropertyDrawer(typeof(CellDrawer))]
public class CellEditor : Editor
{
    public bool showcell = false;
    public bool showSurfaceCell = false;

    public override void OnInspectorGUI()
    {

        base.OnInspectorGUI();

        CellDrawer cd = (CellDrawer)target;


        GUILayout.BeginHorizontal();

        //    if (EditorGUILayout.Box("Show Grid") == true)
        //    else if (EditorGUILayout.Toggle("Show Grid") == false)


            if (showcell == true)
            {
                //cd.OnDrawGismos();

            }
            else if (showcell == false)
            {
                //cd.clear();
            }

        GUILayout.EndHorizontal();



    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

}



