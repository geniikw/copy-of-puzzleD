using UnityEngine;
using System.Collections;

public enum sceneTag
{
    title,
    inGame
}




[System.Serializable]
public class StringPiece  
{
    public string voiceFile;
    public string korean;
    public AudioClip voice;
    public string purpose;
    public BitArray LoadingTag;

    public bool[] serializeBitArray = new bool[2];

    public StringPiece(string vf, string kor, AudioClip vFile, string pp,BitArray sceneFlag)
    {
        voiceFile = vf;
        korean = kor;
        voice = vFile;
        purpose = pp;
        LoadingTag = sceneFlag;

        serializeBitArray[0] = LoadingTag.Get(0);
        serializeBitArray[1] = LoadingTag.Get(1);
    }



}
