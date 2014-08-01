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
    public int drop;

    //접근자
    public UISprite uiSprite { get { return GetComponent<UISprite>(); } }
    public Board board{ get { return GetComponentInParent<Board>(); } }
    Vector3 coordPosition{ get { return board.CoordToScreenPosition(coord); } }
    
    void OnEnable()
    {
        SetRandomColor();
    }
    protected override void OnDragDropStart()
    {
        base.OnDragDropStart();
        //리포지션을 하지 않는다.
        board.bReposition = false;
    }
    protected override void OnDragDropRelease(GameObject surface)
    {
        //board.IgnoreReposition.Remove(this);
        base.OnDragDropRelease(surface);
        //역시 리포지션을 하지 않는다.
        board.bReposition = false;
        //board에서 해당 일처리를 한다.
        board.DectectDestoryElement(); 
    
        //이제 제자리로 들어가야한다.
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

    public void SetRandomColor()
    {
        type = (elementType)Random.Range(0, 6);
        SetColor();
    }
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
    public IEnumerator MoveToMyPosition(float duration)
    {  
        board.IgnoreReposition.Add(this);   
        Vector3 start = transform.localPosition;     //시작좌표를 저장
        Vector3 dest = board.CoordToScreenPosition(coord);
        float t = 0;
        while( t < 1 )
        {
            t += Time.deltaTime / duration;  //t는 0부터 1까지 duration시간동안 오르게 됨
            transform.localPosition = Vector3.Lerp(start, dest, t); //보간 함수
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

    //강의5시작
    public void SendDropMessage()
    {
        int curY = (int)coord.y;
        while(curY > 0)
        {
            curY--;
            board.getElement((int)coord.x, curY).getDropMessage();
        }
    }
    public void getDropMessage()
    {
        drop++;
    }
    public void moveToDeleteLine()
    {
        Vector3 temp = transform.localPosition;
        temp.y = GameCore.instance.deadLine.transform.localPosition.y - drop * 128;
        transform.localPosition = temp;
        SetRandomColor();// 올라갈때 속성을 바꾼다.
    }
    public void genCoord()
    {
        //새로 생성되면서 새로운 좌표를 받는 함수.
        coord = new Vector2(coord.x, drop);
        drop = 0;
    }
    public void dropCoord()
    {   //drop값 만큼 좌표를 내린다.
        coord = new Vector2(coord.x, coord.y + drop);
        drop = 0;
    }
    
}
