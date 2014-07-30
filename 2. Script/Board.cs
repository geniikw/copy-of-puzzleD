using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class Board : UITable {

   
    protected override void Start()
    {
        Init();
        Reposition();
        enabled = false;

        for (int i = 0; i < transform.childCount; ++i)
        {
            Transform child = transform.GetChild(i);
            child.GetComponent<Element>().Coord = new Vector2(i%6,i/6);
            child.name = "Element" + i;
        }

    }
}



#if UNITY_EDITOR
[CustomEditor(typeof(Board))]
public class BoardEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Board myScript = target as Board;
        if (GUILayout.Button("Reposition Button"))
        {
           myScript.Reposition();
        }
    }
}
#endif