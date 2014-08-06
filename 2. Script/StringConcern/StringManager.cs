using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class StringManager : MonoBehaviour {

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

        while ((line = re.ReadLine()) != null)
        {
            splitLine = line.Split(',');
            if (splitLine[0] == "zznone")
                break;
            sTable.Add(splitLine[0], new StringPiece(splitLine[1], splitLine[2], null, splitLine[3]));
        }
        values.AddRange(sTable.Values); //역시나 임시
	}
	
	
}
