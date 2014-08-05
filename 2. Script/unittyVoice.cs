using UnityEngine;

[System.Serializable]
public class unittyVoice
{
    public int index;
    public string caption;
    public AudioClip clip;
    public int playCount;
    public void playSound()
    {
        if (clip != null)
            NGUITools.PlaySound(clip);
    }
    public void playSound(float volumn)
    {
        if (clip != null)
            NGUITools.PlaySound(clip, volumn);
    }
}
