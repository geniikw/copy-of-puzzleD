using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(BoxCollider))]
public class Element : UIDragDropItem{  
    
    public enum elementType
    {
        fire,
        water,
        wood,
        light,
        dark,
        heart,
        nothing
    }
    //맴버 변수
    public elementType type;
    public Vector2 coord;
    //접근자
    public UISprite uiSprite { get { return GetComponent<UISprite>(); } }
    public Board board{ get { return GetComponentInParent<Board>(); } }
    Vector3 coordPosition{ get { return board.CoordToScreenPosition(coord); } }
    
    void OnEnable()
    {
        type = (elementType)Random.Range(0, 6);
        switch (type)
        {
            case elementType.fire:
                uiSprite.color = new Color(0.8f, 0.2f, 0.2f);
                break;
            case elementType.water:
                uiSprite.color = new Color(0.2f, 0.2f, 0.8f);
                break;
            case elementType.wood:
                uiSprite.color = new Color(0.2f, 0.8f, 0.2f);
                break;
            case elementType.light:
                uiSprite.color = new Color(0.8f, 0.8f, 0);
                break;
            case elementType.dark:
                uiSprite.color = new Color(0.5f, 0.1f, 0.8f);
                break;
            case elementType.heart:
                uiSprite.color = new Color(0.8f, 0, 0.8f);
                break;
        }
    }
    protected override void OnDragDropStart()
    {
        //debug
        foreach(Transform a in board.children)
        {
            a.GetComponent<Element>().SetColor();
        }

       // board.IgnoreReposition.Add(this);
        base.OnDragDropStart();
    }
    protected override void OnDragDropRelease(GameObject surface)
    {
       // board.IgnoreReposition.Remove(this);
        base.OnDragDropRelease(surface);
        board.DectectionColumn();
        board.DectectionRow();
        GameCore.instance.label.text = "Destroy Element" + board.destroyElement.Count;
        //이하 디버그용
        foreach (Element a in board.destroyElement)
        {
            a.SetColor(Color.black);
        }


    }

    bool key = false;
    void OnDragOver(GameObject col)
    {       
        if (col != gameObject && !key)
        {
            int first = CopyPDTool.CoordToIndex(coord);
            int second = CopyPDTool.CoordToIndex(col.GetComponent<Element>().coord);
            key = true;               
            
            Vector2 temp = col.GetComponent<Element>().coord;
            col.GetComponent<Element>().coord = coord;
            coord = temp;
         
            board.children.Swap(first, second);
     
            StartCoroutine(CircleMoveDrop(coordPosition, 0.08f));  //이렇게 사용함.
            //board.Reposition();   
        }   
    }


    //디버그용 
    public void SetColor()
    {    
        switch (type)
        {
            case elementType.fire:
                uiSprite.color = new Color(0.8f, 0.2f, 0.2f);
                break;
            case elementType.water:
                uiSprite.color = new Color(0.2f, 0.2f, 0.8f);
                break;
            case elementType.wood:
                uiSprite.color = new Color(0.2f, 0.8f, 0.2f);
                break;
            case elementType.light:
                uiSprite.color = new Color(0.8f, 0.8f, 0);
                break;
            case elementType.dark:
                uiSprite.color = new Color(0.5f, 0.1f, 0.8f);
                break;
            case elementType.heart:
                uiSprite.color = new Color(0.8f, 0, 0.8f);
                break;
        }
    }
    public void SetColor(Color color)
    {
        uiSprite.color = color;
    }

    //코루틴 함수
    IEnumerator MoveDrop(Vector3 dest, float duration)
    {  
        board.IgnoreReposition.Add(this);      
        Vector3 start = transform.localPosition;     //시작좌표를 저장

        float t = 0;
        while( t < 1 )
        {
            t += Time.deltaTime / duration;  //t는 0부터 1까지 duration시간동안 오르게 됨
            transform.localPosition = Vector3.Slerp(start, dest, t); //보간 함수
            yield return null;
        }      
        board.IgnoreReposition.Remove(this);  
        key = false;
    }
    IEnumerator CircleMoveDrop(Vector3 dest, float duration)
    {
        board.IgnoreReposition.Add(this); 
        Vector3 center = (dest + transform.localPosition) / 2;
        Vector3 blueVector = transform.localPosition - center;
        blueVector.z = 0;// 혹시몰라서 제거.    
        //외적을 사용해서 수직백터를 구한다.
        Vector3 redVector = Vector3.Cross(blueVector, new Vector3(0, 0, 1));    
        Vector3 temp;
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / duration;  //t는 0부터 1까지 duration시간동안 오르게 됨
            temp = center + Mathf.Sin(t * Mathf.PI) * redVector + Mathf.Cos(t * Mathf.PI) * blueVector;       
            transform.localPosition = temp; //보간 함수
            yield return null;
        }
        transform.localPosition = dest;
        board.IgnoreReposition.Remove(this);
        key = false;
    }

}
