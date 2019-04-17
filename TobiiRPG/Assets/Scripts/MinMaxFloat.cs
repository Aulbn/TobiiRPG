using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class MinMaxFloat : MonoBehaviour
{
    public float Min;
    public float Max;

    public MinMaxFloat (float min, float max)
    {
        Min = min;
        Max = max;
    }
}
