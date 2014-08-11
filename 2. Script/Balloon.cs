using UnityEngine;
using System.Collections;

public class Balloon : UISprite 
{
    UILabel mString;
    public string Text
    {
        get { return mString.text; }
        set { mString.text = value; }
    }

    void OnEnable()
    {
        mString = transform.GetChild(0).GetComponent<UILabel>();
    }

    public Coroutine Call(string context,float remain,float stime,float etime)
    {
        StopAllCoroutines();
        return StartCoroutine(CallBalloon( context, remain, stime, etime));
    }
    IEnumerator CallBalloon(string context, float remain, float stime, float etime)
    {
        Text = context;
        transform.localScale = Vector3.zero;
        yield return StartCoroutine( ResizeSprite(Vector3.one, stime));
        yield return new WaitForSeconds(remain);
        yield return StartCoroutine(ResizeSprite(Vector3.zero, etime));
    }
    IEnumerator ResizeSprite(Vector3 dest,float time)
    {
        float t = 0;
        Vector3 start = transform.localScale;
        while(t < 1)
        {
            t += Time.deltaTime / time;
            transform.localScale = Vector3.Lerp(start, dest, t);
            yield return null;
        }
    }
   
}
