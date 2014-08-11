using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class StringManager : MonoBehaviour {

    //아이디, 내용,목적,voice파일이름
    const int basicColumnCount = 4;
    const int sceneCount = 2;

    public Dictionary<string, StringPiece> sTable = new Dictionary<string,StringPiece>();
    public static StringManager instance = null;

    //임시 Inspector에서 되나 보려고함
    public List<StringPiece> values = new List<StringPiece>();
    // Use this for initialization
	void Start ()
    {
        instance = this;
        TextAsset temp = Resources.Load<TextAsset>("CopyOfPuzzleAndDragonStringSheet - Sheet1");
        StreamReader re = new StreamReader(new MemoryStream(temp.bytes));
        string line;
        string[] splitLine;

        bool[] a = new bool[sceneCount];

        while ((line = re.ReadLine()) != null)
        {
            splitLine = line.Split(',');
            if (splitLine[0] == "zznone")
                break;

            a[0]= splitLine[4] == "1" ? true : false;
            a[1]= splitLine[5] == "1" ? true : false;

            sTable.Add(splitLine[0], new StringPiece(splitLine[1], splitLine[2], null, splitLine[3],new BitArray(a)));
        }

        //밑에는 임시코드들
        values.AddRange(sTable.Values); 
        StartCoroutine(LoadingVoice((int)sceneTag.title));
	}

    public StringPiece getStringByID(string id)
    {
        StringPiece buffer;
        if(sTable.TryGetValue(id, out buffer)==false)
        {
            Debug.LogError("잘못된 키값 접근 : " + id);
        }
        return buffer;    
    }

    IEnumerator LoadingVoice(int scene)
    {
        WWW www;
        foreach (StringPiece a in sTable.Values)
        {
            a.voice = null;
            if (a.LoadingTag.Get(scene))
            {
                www = new WWW("file://" + Application.persistentDataPath + "/voice/" + a.voiceFile + ".wav");
                yield return www;
                a.voice = www.audioClip;       
            }
        }
        System.GC.Collect();
    }

}


