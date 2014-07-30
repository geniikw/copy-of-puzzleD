using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

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
    public elementType type;
    public UISprite uiSprite
    {
        get { return GetComponent<UISprite>(); }
    }
    public void RandomElement()
    {
        type = (elementType)Random.Range(0, 6);
        syncSprite();
    }

    public Vector2 Coord;

    void syncSprite()
    {
        GetComponent<UISprite>().spriteName = type.ToString();
    }
    void OnEnable()
    {
        RandomElement();
    }

    protected override void OnDragDropStart ()
	{
       // Debug.Log("으하하 시작");
		// Automatically disable the scroll view
		
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
	protected override void OnDragDropMove (Vector3 delta)
	{
		mTrans.localPosition += delta;
        //여기서 섞는다.
	}
	
    protected override void OnDragDropRelease (GameObject surface)
	{	
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
        if(col != gameObject)
        Debug.Log(col+"가 부딧침 에"+transform.name);
    }
}


#if UNITY_EDITOR

public class ElementEditor : Editor
{
    //public override void OnInspectorGUI()
    //{
       
    //   // serializedObject.Update();



    //}
}

#endif