using UnityEngine;
using System.Collections;

public class GameCore : MonoBehaviour 
{

    public UIAtlas mainResourceAtlas;
    public Element elementPrefab;

    public static GameCore instance;

	// Use this for initialization
	void Awake ()
    {
        instance = this;
	}
}
