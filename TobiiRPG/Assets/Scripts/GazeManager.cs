﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii;
using Tobii.GameIntegration;
using Tobii.Gaming;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GazeManager : MonoBehaviour
{
    public static GazeManager Instance;
    private GazePoint gazePoint { get { return TobiiAPI.GetGazePoint(); } }
    public static Vector2 GazePosition { get { return Instance.gazePoint.Screen; } }
    public static bool IsConnected { get { return TobiiAPI.IsConnected; } }
    //public static float SelectionTime;
    public float selectionErrorRadius;
    private bool wasGazeAboveScreen = false;

    // Events
    public static Action OnEyeGestureAboveScreen;

    private void Start()
    {
        Instance = this;
        //int a = 5;
        //OnSomething += () => Debug.Log(a + "x");
        //OnSomething += () => Debug.Log("xxx");
        //Debug.Log("Before event");
        //OnSomething();
    }

    private void Update()
    {
        EyeGestureUpdate();
    }

    private void EyeGestureUpdate()
    {
        if (!wasGazeAboveScreen && GazePosition.y > Screen.height)
        {
            wasGazeAboveScreen = true;
            OnEyeGestureAboveScreen();
        }
        else if (wasGazeAboveScreen && GazePosition.y < Screen.height)
            wasGazeAboveScreen = false;
            
    }

    public static Collider RaycastCollider()
    {
        if (GazePosition.x < 0 || GazePosition.x > Screen.width || GazePosition.y < 0 || GazePosition.y > Screen.height)
            return null;
        Ray ray = Camera.main.ScreenPointToRay(GazePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
            return hit.collider;
        else
            return null;
    }

    public static Vector3 RaycastPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(GazePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
            return hit.point;
        else
            return Vector3.zero;
    }

    public static Graphic RaycastUI()
    {
        PointerEventData eventData = new PointerEventData(UIManager.eventSystem);
        eventData.position = GazePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        UIManager.graphicRaycaster.Raycast(eventData, results);
        foreach (RaycastResult result in results)
        {
            Graphic sel = result.gameObject.GetComponent<Graphic>();
            if (sel != null)
                return sel;
        }
        return null;
    }
}
