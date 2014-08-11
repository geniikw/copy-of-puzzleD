using UnityEngine;
using System.Collections;

public class DeathEffect : MonoBehaviour 
{
    Color color;
    public Color Color
    {
        get { return color; }
        set 
        {
            color = value;
            for (int n = 0; n < transform.childCount; n++)
            {
                transform.GetChild(n).GetComponent<TweenColor>().to = Color;
            }
        }
    }


    void OnEnable()
    {
        for (int n = 0; n < transform.childCount; n++)
        {
            transform.GetChild(n).GetComponent<TweenColor>().to = Color;
        }

        UITweener[] temp;
        for (int n = 0; n < transform.childCount; n++)
        {
            temp=  transform.GetChild(n).GetComponents<UITweener>();
            foreach(UITweener a  in temp)
            {
                a.ResetToBeginning();
                a.PlayForward();
            }
        }    
    }	
}
