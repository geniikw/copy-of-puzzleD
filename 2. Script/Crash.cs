using UnityEngine;
using System.Collections;
using System.Collections.Generic;//List 쓰기위해 필요.

[System.Serializable]
public class Line : List<Element>
{
    public List<Line> listNaver = new List<Line>();   
    public void addNaver(Line add)
    {
        listNaver.Add(add);
    }
  
}
