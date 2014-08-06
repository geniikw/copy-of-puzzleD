using UnityEngine;
using System.Collections;

public class GameCore : MonoBehaviour 
{
    public UILabel[] label;

    public Transform deadLine;
    public UICamera uiCamera;

    public enum gameState
    {
        STARTING,//시작중
        WAITING, //터치가 불가능한 상태
        PLAYING, //터치가 가능한 상태
        EXITING  //종료중
    }

    gameState state;

    gameState State
    {
        get { return state; }
        set 
        {
            wait = 0f;
            state = value;
        }
    }

    public int score;
    public int hp;
    public int combo;
    [SerializeField] int randomPose;
    public float wait;
    public int RandomPose
    {
        get
        {
            int temp = randomPose;
            randomPose = 0;
            return temp;
        }
    }

    public static GameCore instance;
	// Use this for initialization
	void Awake ()
    {
        instance = this;
        state = gameState.PLAYING;
	}

    void Update()
    { 
       if(state == gameState.PLAYING)
       {
           wait += Time.deltaTime;
           if(wait > 5f)
           {
               randomPose = Random.Range(0, 5);
               wait = 0;
           }
       }
       label[0].text = "Score : " + score;
       //label[1].text = "Comboe : " + combo;
    }
    public void getTouchMessage()
    {
        wait = 0;
    }

}

