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
    public Element[] chain = new Element[4];
    public bool bComboFlag = false;

    public void chainClear()
    {
        for(int n = 0 ; n < 4 ; n++)
        {
            chain[n] = null;
        }
    }

    //접근자
    public UISprite uiSprite { get { return GetComponent<UISprite>(); } }
    public Board board{ get { return GetComponentInParent<Board>(); } }
    Vector3 coordPosition{ get { return board.CoordToScreenPosition(coord); } }

    Vector2 rightCoord { get { return new Vector2(coord.x + 1, coord.y); } }
    Vector2 leftCoord { get { return new Vector2(coord.x - 1, coord.y); } }
    Vector2 upCoord { get { return new Vector2(coord.x, coord.y-1); } }
    Vector2 downCoord { get { return new Vector2(coord.x, coord.y+1); } }

    void OnEnable()
    {
        SetRandomColor();
    }
    protected override void OnDragDropStart()
    {
       transform.position = UICamera.currentCamera.ScreenToWorldPoint(Input.mousePosition);
  
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
        //제자리로 돌아감
        StartCoroutine(MoveToMyPosition(0.1f));

        StartCoroutine(turnEndSeq());     
    }
    IEnumerator turnEndSeq()
    {
        yield return board.DectectDestoryElement();       
    }
    
    bool key = false;
    void OnDragOver(GameObject col)
    {       
        if (col != gameObject && !key)
        {          
            key = true;
            CopyPDTool.SwapCoord(col.GetComponent<Element>(), this);
            AudioSource.PlayClipAtPoint(GameCore.instance.swapSound,transform.position);
            int first = CopyPDTool.CoordToIndex(coord);
            int second = CopyPDTool.CoordToIndex(col.GetComponent<Element>().coord);
            board.children.Swap(first, second);    

            StartCoroutine(CircleMoveDrop(coordPosition, 0.08f));   
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
        SetupChain();
       
    }
    public void getDropMessage()
    {
        drop++;
    }  
    public Coroutine DeadSelf(float time)
    {
       return StartCoroutine(Dead(time));
    }
    
    public IEnumerator Dead(float time)
    {
        GameCore.instance.requestDeathEffect(this);
        yield return StartCoroutine(SetAlphaValueForSecond(0f, time));
        GameCore.instance.score += (int)(10f * (GameCore.instance.combo/10f + 1f));
       
        Vector3 temp = transform.localPosition;
        temp.y = GameCore.instance.deadLine.transform.localPosition.y - drop * board.uiSprite.height/5;
        transform.localPosition = temp;       
        coord = new Vector2(coord.x, drop);
        gameObject.SetActive(false);

        drop = 0;
    }  
    public IEnumerator Alive(float time)
    {
        bComboFlag = false;
        SetRandomColor();//속성을 바꾼다.
        gameObject.SetActive(true);
       
        Transform de = transform.GetChild(0);

        if(de != null)
        {
            de.GetComponent<DeathEffect>().Color = uiSprite.color;
        }

        yield return StartCoroutine(SetAlphaValueForSecond(1f, time));     
    }         
    public void dropCoord()
    {   //drop값 만큼 좌표를 내린다.       
        coord = new Vector2(coord.x, coord.y + drop);
        drop = 0;
    }

    public void SetupChain()
    {
        int index = 0;
        if (board.getElementType(upCoord) == type)
        {
            chain[index++] = board.getElement(upCoord);
        }
        if (board.getElementType(downCoord) == type)
        {
            chain[index++] = board.getElement(downCoord);
        }
        if (board.getElementType(rightCoord) == type)
        {
            chain[index++] = board.getElement(rightCoord);
        }
        if (board.getElementType(leftCoord) == type)
        {
            chain[index++] = board.getElement(leftCoord);
        }
    }  
}
