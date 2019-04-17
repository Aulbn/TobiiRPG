using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public static bool EyeTracking = false;
    public enum gameStates
    {

    }

    private void Awake()
    {
        Instance = this;
    }
}
