using UnityEngine;
using System.Collections;

public class Fade : MonoBehaviour
{
    public Texture2D blackRect;
    public int drawDepth = -1000;
    float alpha = 1f;//초기 알파값  

    public Color color;

    void Awake()
    {
        color = new Color(1, 1, 1, 1f);
    }

    void Start()
    {
        //StartCoroutine(fadeCoroutine(0, 5));
        //caution!!
        //Time.realTimeSinceStartup is initilize after Awake() done; //시발 내가 쓴거였넹 ㅡㅡ
    }

    void OnGUI()
    {
        GUI.depth = drawDepth;
        GUI.color = color;

        if (blackRect != null)
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), blackRect);
    }

    public Coroutine FadeOut(float time)
    {
        return StartCoroutine(fadeCoroutine(1, time));
    }
    public Coroutine FadeIn(float time)
    {
        return StartCoroutine(fadeCoroutine(0, time));
    }

    IEnumerator fadeCoroutine(float dest, float time)
    {
        float t = 0;
        float start = alpha;
        float current = Time.realtimeSinceStartup;
        while (t < 1)
        {
            t += (Time.realtimeSinceStartup - current) / time;
            current = Time.realtimeSinceStartup;
            alpha = Mathf.Lerp(start, dest, t);
            color.a = alpha;
            yield return null;
        }
    }
}
