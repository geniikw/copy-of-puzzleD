using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class AnimatorBind : MonoBehaviour 
{
    public Animator anim;
    public UIPlaySound[] sound;

    int ScoreCounter;
    int comboCounter;
	// Use this for initialization
	void Start () 
    {
        anim = GetComponent<Animator>();
        sound = GetComponents<UIPlaySound>();
	}
	
	// Update is called once per frame
	void LateUpdate () 
    {     
  
	}
}
