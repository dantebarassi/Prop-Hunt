using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extentions 
{
    public static Vector3 SetXAxis(this Vector3 vector, float xValue)
    {
        return new Vector3(xValue, vector.y, vector.z);
    }
    public static Vector3 SetY0(this Vector3 vector)
    {
        vector.y = 0;
        return vector;
    }
}
