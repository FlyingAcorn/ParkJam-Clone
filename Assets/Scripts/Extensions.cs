using System;
using UnityEngine;

public abstract class Extensions
{
    public static Vector3 QuadraticLerp(Vector3 start, Vector3 middle, Vector3 end, float t)
    {
        Vector3 startAndMiddle = Vector3.Lerp(start, middle, t);
        Vector3 middleAndEnd = Vector3.Lerp(middle, end, t);
        return Vector3.Lerp(startAndMiddle, middleAndEnd, t);
    }


    public static void UpdateInt(string name, Func<int, int> func)
    {
        PlayerPrefs.SetInt(name, func.Invoke(PlayerPrefs.GetInt(name)));
    }
}