using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class CopyPDTool 
{
    public static void SwapCoord(Element a , Element b)
    {
        Vector2 temp = b.coord;
        b.coord = a.coord;
        a.coord = temp;
    }
	public static int CoordToIndex(Vector2 coord)
    {
        return ((int)coord.x % 6 + (int)coord.y * 6);  
    }
    public static int CoordToIndex(int x , int y)
    {
        return x % 6 + y * 6;
    }
    public static Vector2 IndexToCoord(int index)
    {
        return new Vector2(index % 6, (int)(index / 6));
    }
    public static IList<T> Swap<T>(this  IList<T> list,
        int index1,
        int index2)
    {
        T temp = list[index1];
        list[index1] = list[index2];
        list[index2] = temp;
        return list;
    }
}
