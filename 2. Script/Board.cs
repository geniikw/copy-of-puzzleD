using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class Board : UITable 
{

}
#if UNITY_EDITOR
[CustomEditor(typeof(Board))]
public class BoardEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        //UITable에서 Reposition을 버튼으로 사용하게 만듬
        Board myScript = target as Board;
        if (GUILayout.Button("Reposition Button"))
        {
           myScript.Reposition();
        }
    }
}
#endif