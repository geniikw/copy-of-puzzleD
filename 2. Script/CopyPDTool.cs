using UnityEngine;
using System.Collections;

public class CopyPDTool {

	public static int CoordToIndex(Vector2 coord)
    {
        return ((int)coord.x % 6 + (int)coord.y * 6);  
    }

    public static Vector2 IndexToCoord(int index)
    {
        return new Vector2(index % 6, (int)(index / 6));
    }
   
}
