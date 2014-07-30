using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]

public class Element : UIDragDropItem{

    public enum elementType
    {
        fire,
        water,
        wood,
        light,
        dark,
        heart
    }

    //맴버 변수
    public elementType type;
    public Vector2 coord;

    public UISprite uiSprite { get { return GetComponent<UISprite>(); } }
    public Board board
    { get { return GetComponentInParent<Board>(); } }

    public void SetRandomElement()
    {
        type = (elementType)Random.Range(0, 6);
        syncSprite();
    }
    void syncSprite()
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
        //만약 다른 스프라이트로 변경하고 싶다면 이런 방식으로 하면 좋다. 아틀라스 내에 스프라이트의 이름이 같아야함
        //GetComponent<UISprite>().spriteName = type.ToString();

    }
    void OnEnable()
    {
        SetRandomElement();
    }

    protected override void OnDragDropStart()
    {
    //    Debug.Log("으하하 시작");
        board.curDrag = this;

        // Disable the collider so that it doesn't intercept events
        if (mButton != null) mButton.isEnabled = false;
        else if (mCollider != null) mCollider.enabled = false;

        mTouchID = UICamera.currentTouchID;
        mParent = mTrans.parent;
        mRoot = NGUITools.FindInParents<UIRoot>(mParent);
        mTable = NGUITools.FindInParents<UITable>(mParent);

        // Re-parent the item
        if (UIDragDropRoot.root != null)
            mTrans.parent = UIDragDropRoot.root;

        Vector3 pos = mTrans.localPosition;
        pos.z = 0f;
        mTrans.localPosition = pos;

        // Notify the widgets that the parent has changed
        NGUITools.MarkParentAsChanged(gameObject);

        if (mTable != null) mTable.repositionNow = true;
    }
    protected override void OnDragDropMove(Vector3 delta)
    {
        mTrans.localPosition += delta;     
    }
    protected override void OnDragDropRelease(GameObject surface)
    {
        board.curDrag = null;

        mTouchID = int.MinValue;
        // Re-enable the collider
        if (mButton != null) mButton.isEnabled = true;
        else if (mCollider != null) mCollider.enabled = true;

        // Is there a droppable container?
        UIDragDropContainer container = surface ? NGUITools.FindInParents<UIDragDropContainer>(surface) : null;

        if (container != null)
        {
            // Container found -- parent this object to the container
            mTrans.parent = (container.reparentTarget != null) ? container.reparentTarget : container.transform;

            Vector3 pos = mTrans.localPosition;
            pos.z = 0f;
            mTrans.localPosition = pos;
        }
        else
        {
            // No valid container under the mouse -- revert the item's parent
            mTrans.parent = mParent;
        }

        // Update the grid and table references
        mParent = mTrans.parent;
        mTable = NGUITools.FindInParents<UITable>(mParent);

        // Notify the widgets that the parent has changed
        NGUITools.MarkParentAsChanged(gameObject);

        if (mTable != null) mTable.repositionNow = true;
    }

    void OnDragOver(GameObject col)
    {
        if (col != gameObject)
        {
            Vector2 temp = col.GetComponent<Element>().coord;
            col.GetComponent<Element>().coord = GetComponent<Element>().coord;
            GetComponent<Element>().coord = temp;
            board.Reposition();
        }       
    }
}
