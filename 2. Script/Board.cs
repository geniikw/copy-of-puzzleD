using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class Board : UITable 
{
    const int column = 6;
    const int row = 5;
    
    //단순히 UITable에서 Start를 쓰고 있기떄문에 Awake를 씀...
    void Awake()
    {
        for (int n = 0; n < transform.childCount; n++)
        {
            Transform child = transform.GetChild(n);
            child.transform.name = "Element" + n;
            child.GetComponent<Element>().coord = CopyPDTool.IndexToCoord(n);
        }
    }
    protected override void Sort(List<Transform> list)
    {
        list.Sort(compareCoord);
    }
    public UISprite uiSprite { get { return GetComponent<UISprite>(); } }
    int compareCoord(Transform a, Transform b)
    {         //a가 작으면 -1 같으면 버그 a가 크면 1을 리턴하면 될듯ㅇㅇ;     
            Vector2 aCoord = a.GetComponent<Element>().coord;
            Vector2 bCoord = b.GetComponent<Element>().coord;
            return CopyPDTool.CoordToIndex(aCoord).CompareTo(CopyPDTool.CoordToIndex(bCoord));
    }
   
    //강의3 시작

    //이 List에 등록된 Element는 Reposition 작업시 위치를 변경하지 않는다.
    public List<Element> IgnoreReposition = new List<Element>();
    public override void Reposition()
    {
        Vector3[] temp = new Vector3[IgnoreReposition.Count];
        for (int n = 0; n < IgnoreReposition.Count; n++ )
        {
            temp[n] = IgnoreReposition[n].transform.localPosition;
        }
        base.Reposition();        
        for (int n = 0; n < IgnoreReposition.Count; n++)
        {
            IgnoreReposition[n].transform.localPosition = temp[n];
        }
    }

    protected void RepositionVariableSize(List<Transform> children)
    {
        Debug.Log("dd");
        base.RepositionVariableSize(children);
    }
    //유틸리티 함수 이게 필요함
    public Vector3 CoordToScreenPosition(Vector2 coord)
    {
        float blockWidth = uiSprite.width / column;
        float blockHeight = uiSprite.height / row;
        return new Vector3(blockWidth / 2 + blockWidth * coord.x, -blockHeight / 2 - blockHeight * coord.y, 0);
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