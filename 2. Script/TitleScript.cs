using UnityEngine;
using System.Collections;

public class TitleScript : MonoBehaviour {

    public Texture2D[] LogoArray;
    public Color[] LogoBackArray;
    public float[] LogoRemain;
    public float fadeOutTime = 3;

    Color s = new Color(1, 1, 1, 0);
    Color e = new Color(1, 1, 1, 1);

    Fade fadeComponent;
    Texture2D currentLogo = null;
    Rect currentRect;
    Color currentColor;
    float currentRemain;

    // Use this for initialization
	void Start () 
    {
        currentColor = new Color(0, 0, 0, 0);
        fadeComponent = Camera.main.GetComponent<Fade>();
        StartCoroutine(titleSequence());
        StartCoroutine(GetSkip());
	}
    IEnumerator GetSkip()
    {
        while(true)
        {
            if(Input.GetMouseButton(0))
            {
                Time.timeScale = 5;
            }
            else
            {
                Time.timeScale = 1;
            }
            yield return null;
        }
    }
	IEnumerator titleSequence()
    {
        GameCore.instance.uiCamera.useTouch = false;
        GameCore.instance.uiCamera.useMouse = false;


        yield return new WaitForEndOfFrame();

        for( int n =0 ; n < LogoArray.Length; n ++)
        {
            yield return StartCoroutine(LogoView(LogoArray[n],LogoBackArray[n],LogoRemain[n]));
        }
        currentLogo = null;     
        //다 끝나면 끈다.
        yield return StartCoroutine(fadeControl(e, Color.clear, fadeOutTime));

        GameCore.instance.uiCamera.useTouch = true;
        GameCore.instance.uiCamera.useMouse = true;

        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
    IEnumerator LogoView(Texture2D tex, Color col, float time)
    {
        currentRect = new Rect(Screen.width /10f, Screen.height / 3f, Screen.width / 10 * 8, Screen.height/3);
        currentLogo = tex;
        yield return StartCoroutine(fadeControl(e, col, time));
        yield return StartCoroutine(logoControl(s, e, time));
        yield return new WaitForSeconds(time);
        yield return StartCoroutine(logoControl(e, s, time));
        yield return StartCoroutine(fadeControl(col, e, time));
    }
    void OnGUI()
    {
        if(currentLogo != null)
        {
            GUI.depth = -1001;//더 깊게?
            GUI.color = currentColor;
            GUI.DrawTexture(currentRect, currentLogo);
        }
    }
    IEnumerator fadeControl(Color start, Color end, float time)
    {
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime/time;
            fadeComponent.color = Color.Lerp(start, end, t);
            yield return null;
        }
    }
    IEnumerator logoControl(Color start, Color end, float time)
    {
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / time;
            currentColor = Color.Lerp(start, end, t);
            yield return null;
        }
    }
}
