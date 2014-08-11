using UnityEngine;
using System.Collections;

public enum ingame_string_id
{
    BYE_00,
BYE_01,
BYE_02,
BYE_03,
BYE_QUESTION_00,
CLEAR_00,
COUNT_EIGHT,
COUNT_FIVE,
COUNT_FOUR,
COUNT_NINE,
COUNT_ONE,
COUNT_SEVEN,
COUNT_SIX,
COUNT_TEN,
COUNT_THREE,
COUNT_TWO,
COUNT_ZERO,
GAMEOVER_00,
GAMEOVER_01,
GP_BAD_00,
GP_BAD_01,
GP_BAD_02,
GP_COMBO_HIGH_00,
GP_COMBO_HIGH_01,
GP_EXCELLENT_00,
GP_GOOD_00,
GP_GOOD_01,
GP_GOOD_02,
GP_NICE_00,
GP_NICE_01,
GP_NICE_02,
GP_NICE_03,
READY_00,
READY_01,
RESULT_00,
START_00
}
public enum title_string_id
{
    AFTERNOON_00,
BYE_00,
BYE_01,
BYE_02,
BYE_03,
EVENING_00,
GREETING_00,
MORNING_00,
OPENING_MENT_00,
POWER_BY_UNITY_00,
REACTION_00,
REACTION_01,
REACTION_02,
REACTION_03,
REACTION_04,
REACTION_05
}


public class GameCore : MonoBehaviour 
{
    public UILabel[] label;
    public AudioClip swapSound;
  
    public Transform deadLine;
    public UICamera uiCamera;

    public DeathEffect prefab;
    public DeathEffect[] deArray;
    public int curDe;
    int MAX_DEATH_EFFECT = 20;

    public unityChan unityChanPointer;

    public DeathEffect requestDeathEffect(Element use)
    {
        DeathEffect r = deArray[curDe++];
        if (curDe == MAX_DEATH_EFFECT)
            curDe = 0;
      
        r.Color = use.uiSprite.color;
        r.transform.parent = use.transform;
        r.transform.localPosition = Vector3.zero;
        r.gameObject.SetActive(false);
        r.gameObject.SetActive(true);
        return r;
    }


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

        deArray = new DeathEffect[MAX_DEATH_EFFECT];
        for (int n = 0; n < MAX_DEATH_EFFECT; n++)
        {
            deArray[n] = Instantiate(prefab) as DeathEffect;
            deArray[n].gameObject.SetActive(false);
            deArray[n].transform.parent = transform;
        }

	}

    void Update()
    { 
       if(state == gameState.PLAYING)
       {
           wait += Time.deltaTime;
           if(wait > 10f)
           {
               unityChanPointer.MotionSelector((unityChan.unitychanMotion)Random.Range(0,4));
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

