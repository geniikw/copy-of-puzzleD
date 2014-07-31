using UnityEngine;
using System.Collections;

public class GameCore : MonoBehaviour 
{
    public UILabel label;
    
    
    public static GameCore instance;
	// Use this for initialization
	void Awake ()
    {
        instance = this;
	}
}
