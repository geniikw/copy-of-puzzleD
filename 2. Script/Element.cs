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
    public List<Element> listGroup = new List<Element>();

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
        GameCore.instance.getTouchMessage();
        base.OnDragDropStart();
        //리포지션을 하지 않는다.
        board.bReposition = false;
    }
    protected override void OnDragDropRelease(GameObject surface)
    {
        GameCore.instance.getTouchMessage();
        //board.IgnoreReposition.Remove(this);
        base.OnDragDropRelease(surface);
        //역시 리포지션을 하지 않는다.
        board.bReposition = false;
        StartCoroutine(turnEndSeq());   
    }
    IEnumerator turnEndSeq()
    {
        //터치를 막음
        GameCore.instance.uiCamera.useMouse = false;
        GameCore.instance.uiCamera.useTouch = false;
        //제자리로 돌아감
        StartCoroutine(MoveToMyPosition(0.1f));
        //board가 다끝나면 알려줌
        yield return board.DectectDestoryElement();
        //터치를 품
        GameCore.instance.uiCamera.useMouse = true;
        GameCore.instance.uiCamera.useTouch = true;
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
        //alpha값을 유지해야함
        float tempAlpha = uiSprite.alpha;
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
        uiSprite.alpha = tempAlpha;
    }
    public void SetColor(Color color)
    {
        uiSprite.color = color;
    }

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
    public IEnumerator CircleMoveDrop(Vector3 dest, float duration)
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
    public IEnumerator SetAlphaValueForSecond(float alpha, float duration)
    {
        float t = 0;
        float start = uiSprite.alpha;
        while(t < 1)
        {
            t += Time.deltaTime / duration;
            uiSprite.alpha = Mathf.Lerp(start, alpha,t);         
            yield return null;
        }
    }

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
    
    public IEnumerator Dead(float time)
    {       
        yield return StartCoroutine(SetAlphaValueForSecond(0f, time));

        GameCore.instance.score += 10;

        Vector3 temp = transform.localPosition;
        temp.y = GameCore.instance.deadLine.transform.localPosition.y - drop * board.uiSprite.height/5;
        transform.localPosition = temp;       
        coord = new Vector2(coord.x, drop);
        drop = 0;    
    }  
    public IEnumerator DeadAll(float time)
    {
        if (listGroup.Count == 0)
            Debug.LogError("잘못된 사용 : " + gameObject.name);

        foreach (Element a in listGroup)
        {   
            StartCoroutine(a.Dead(time));
        }    
        //다썼으니 초기화
        listGroup.Clear();
        yield return new WaitForSeconds(time);
    }

    public IEnumerator Alive(float time)
    {
        SetRandomColor();//속성을 바꾼다.
        yield return StartCoroutine(SetAlphaValueForSecond(1f, time));
      
    }
    
      
    public void dropCoord()
    {   //drop값 만큼 좌표를 내린다.
       
        coord = new Vector2(coord.x, coord.y + drop);
        drop = 0;
    }
   
    
}
