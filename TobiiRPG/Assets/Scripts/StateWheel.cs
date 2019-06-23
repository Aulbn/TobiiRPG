using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateWheel : MonoBehaviour
{
    public Image buttWalk, buttInter, buttExit;
    private RectTransform rect;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        rect.position = UIManager.InsideScreenPos(UIManager.TargetMarkerPosition(), rect.sizeDelta.x/2);

        Graphic graphic = GazeManager.RaycastUI();
        if (graphic == null)
            return;

        if (graphic.Equals(buttWalk))
        {
            Debug.Log("Walk state");
            PlayerController.SetState(PlayerController.PlayerState.MoveState);
            CloseWheels();
        }
        else if (graphic.Equals(buttInter))
        {
            Debug.Log("Inter state");
            PlayerController.SetState(PlayerController.PlayerState.IdleState);
            CloseWheels();
        }
        else if (graphic.Equals(buttExit))
        {
            //Exit
            //Debug.Log("Exit");
            CloseWheels();
        }

    }

    private void CloseWheels()
    {
        UIManager.ShowSpellWheel(false);
        UIManager.ShowStateWheel(false);
    }

}
