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
        anim.SetInteger("HP", GameCore.instance.hp);
        anim.SetInteger("RandomWaitPose", GameCore.instance.RandomPose);   
        if(GameCore.instance.score - ScoreCounter > 200)
        {
            anim.SetTrigger("Score");
            sound[0].Play();
            ScoreCounter += 200;
        }
        if(GameCore.instance.combo - comboCounter == 6)
        {
            sound[1].Play();
            anim.SetTrigger("Combo");
            comboCounter += 6;
        }

	}
}
