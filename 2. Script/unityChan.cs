using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class unityChan : MonoBehaviour
{
    int love;
    StringManager mgr;
    AudioSource sound;
    Animator anim;
    public Balloon balloon;

    public enum unitychanFace
    {
        smile1,
        sap,
        angry2,
        smile2,
        SURPRISE,
        disstract1,
        disstract2,
        angry1,
        ASHAMED,
        eye_close,
        conf
    }
    public enum unitychanMotion
    {
        wait1,
        wait2,
        wait3,
        kick4
    }
    public enum unitychanStaticMotion
    {
        GOOD1,
        GOOD2,
        GOOD3,
        EXCEL1,
        EXCEL2,
        FANTA1,
        FANTA2,
        FANTA3,
        ETC1,
        ETC2,
        ETC3
    }

    public enum unitychanCount
    {
        COUNT_ZERO,
        COUNT_ONE,
        COUNT_TWO,
        COUNT_THREE,
        COUNT_FOUR,
        COUNT_FIVE,
        COUNT_SIX,
        COUNT_SEVEN,
        COUNT_EIGHT,
        COUNT_NINE,
        COUNT_TEN
    }


    //Face StartCorotine
    IEnumerator currentFaceCorotine;
    Coroutine StartFaceCorotine(IEnumerator routine)
    {
        currentFaceCorotine = routine;
        return StartCoroutine(routine);
    }
    void StopFaceCorotine()
    {
        if(currentFaceCorotine != null)
         StopCoroutine(currentFaceCorotine);
    }
    //Motion StartCoroutine
    IEnumerator currentMotionCorotine;
    Coroutine StartMotionCorotine(IEnumerator routine)
    {
        currentMotionCorotine = routine;
        return StartCoroutine(routine);
    }
    bool StopMotionCorotine()
    {
        if (currentMotionCorotine != null)
        {
            StopCoroutine(currentMotionCorotine);
            return true;
        }
        return false;
    }
    
    //initialize
    void Start()
    {
        //목소리관련 source//여기서 source는 원본이 아니라 소리가 나는곳이란 뜻인듯.
        sound = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        mgr = StringManager.instance;
    }
    //Routine for face
    IEnumerator cfChangeFace(unitychanFace face,float remain,float sTime,float eTime)
    {
        FaceSelector(face);
        yield return StartFaceCorotine(changeAnimWeight(1, sTime));
        yield return new WaitForSeconds(remain);
        yield return StartFaceCorotine(changeAnimWeight(0, eTime));
        currentFaceCorotine = null;
    }
    void FaceSelector(unitychanFace face)
    {
        anim.Play(face.ToString(), 1);
        anim.SetLayerWeight(1, 0);
    }   
    IEnumerator changeAnimWeight(float dest ,float duration)
    {
        float t = 0;
        float start = anim.GetLayerWeight(1);
        while(t < 1)
        {
            t += Time.deltaTime / duration;
            anim.SetLayerWeight(1, Mathf.Lerp(start, dest, t));
            yield return null;
        }
    }
    //Routine for Static Motion
    IEnumerator ssStaticMotionSelector(unitychanStaticMotion motion,float sTime,float remain)
    {
        anim.CrossFade(motion.ToString(), sTime);
        yield return StartMotionCorotine(waitFuntion(remain));
        anim.SetTrigger("WaitTrigger");
        currentMotionCorotine = null;
    }
    IEnumerator waitFuntion(float time)
    {   
        float t = 0;
        while(t < 1)
        {
            t += Time.deltaTime / time;
            yield return null;
        }
    }

    //외부 호출함수
    public Coroutine ChangeFace(unitychanFace face,float remain=3f,float sTime=0.3f,float eTime=0.3f)
    {
        return StartFaceCorotine(cfChangeFace(face, remain, sTime, eTime));
    }
    public void MotionSelector(unitychanMotion motion)
    {
        anim.Play(motion.ToString(), 0, 5);
    }
    public Coroutine StaticMotionSelector(unitychanStaticMotion motion,float sTime,float remain)
    {
        return StartMotionCorotine(ssStaticMotionSelector(motion, sTime, remain));
    }
     
    public void Speak(string id,float remain = 3f)
    {
        int randomFace = Random.Range(0, 3);
        ChangeFace((unitychanFace)randomFace, 2f);
        
        
        StringPiece temp = StringManager.instance.getStringByID(id);
        balloon.Call(temp.korean, remain, 0.3f, 0.3f);
        if(temp.voice == null)
        {
            Debug.LogError("로딩되지 않은 목소리파일 사용 : " + temp.voiceFile);
        }
        else
        {
            sound.clip = temp.voice;
        }
        sound.Play();
    }

    //각각의 0,1,2,3 에따라서 표정 대사가 달라짐
    public void ComboReaction(int combo)
    {
        ///으아아아아아 코통 받는다아아아아아!!!! 대충짬... 음 이걸 따로 뺴서... 쿨럭...
        int temp = Random.Range(0, 3);//0,1,2
        if(combo < 11 && combo > 0)
        {
            temp = Random.Range(0, 5);//0,1,2,3,4
            switch (temp)
            {
                case 0:
                case 1:
                    Speak(((unitychanCount)combo).ToString(), 1f);      
                    break;
                case 2:
                    Speak("GP_GOOD_00");
                    break;
                case 3:
                    Speak("GP_GOOD_01");
                    break;
                case 4:
                    Speak("GP_GOOD_02");
                    break;
            }        
        }
        else
        {
            temp = Random.Range(0, 3);//0,1,2
            switch( temp)
            {
                case 0:
                    Speak("GP_COMBO_HIGH_00");
                    break;
                case 1:
                    Speak("GP_COMBO_HIGH_01");
                    break;
                case 2:
                    Speak("GP_EXCELLENT_00");
                    break;
            }
        }

        if (temp > 1)
        {
            temp = Random.Range(0, 3);
            StaticMotionSelector((unitychanStaticMotion)temp, 0.3f, 2f);
        }
    }
    //debug...
    unitychanStaticMotion temp = 0;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            if(StopMotionCorotine())
            {
                Debug.Log("b");
                StaticMotionSelector(temp, 10f, 2f);
            }
            else
            {
                Debug.Log("a");
                StaticMotionSelector(temp, 0.1f, 2f);
            }
            temp += 1;
        }
        
    }

}
