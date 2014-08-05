using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VoiceManager : MonoBehaviour 
{
    public List<unittyVoice> voices = new List<unittyVoice>();
    static public VoiceManager instance;
	void Start () 
    {     
        instance = this;  
    //    path = Application.persistentDataPath + "/Voice/univ1014.wav";
    //    WWW www = new WWW("file://" + path);
    //    yield return www;   
    }	
}
