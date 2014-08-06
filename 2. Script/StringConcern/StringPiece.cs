using UnityEngine;
using System.Collections;

[System.Serializable]
public class StringPiece  
{
    public string voiceFile;
    public string korean;
    public AudioClip voice;
#if UNITY_EDITOR
    public string purpose;
#endif

    public StringPiece(string vf, string kor, AudioClip vFile, string pp)
    {
        voiceFile = vf;
        korean = kor;
        voice = vFile;
        purpose = pp;
    }

}
