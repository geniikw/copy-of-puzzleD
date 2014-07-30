using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class Board : UITable 
{
    //단순히 UITable에서 Start를 쓰고 있기떄문에 Awake를 씀...
    void Awake()
    {
        for (int n = 0; n < transform.childCount; n++)
        {
            Transform child = transform.GetChild(n);
            child.GetComponent<Element>().coord = CopyPDTool.IndexToCoord(n);
        }
    }
    protected override void Sort(List<Transform> list)
    {
        list.Sort(compareCoord);
    }

    int compareCoord(Transform a, Transform b)
    {
        //a가 작으면 -1 같으면 버그 a가 크면 1을 리턴하면 될듯ㅇㅇ;
        Vector2 aCoord = a.GetComponent<Element>().coord;
        Vector2 bCoord = b.GetComponent<Element>().coord;
        return CopyPDTool.CoordToIndex(aCoord).CompareTo(CopyPDTool.CoordToIndex(bCoord));
    }

    public Element curDrag;
    public override void Reposition()
    {
        Vector3 tempLP = Vector3.zero;
        if (curDrag != null)
        {
            tempLP = curDrag.transform.localPosition;
        }
        base.Reposition();
        
        if(curDrag !=null)
        {
         curDrag.transform.localPosition = tempLP;
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

        //UITable에서 Reposition을 버튼으로 사용하게 만듬
        Board myScript = target as Board;
        if (GUILayout.Button("Reposition Button"))
        {
           myScript.Reposition();
        }
    }
}
#endif