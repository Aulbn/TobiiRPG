using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellWheel : MonoBehaviour
{
    public Image buttSpell1, buttSpell2, buttExit;
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

        if (graphic.Equals(buttSpell1))
        {
            PlayerController.ThrowSpell(1);
            CloseWheel();
        }
        else if (graphic.Equals(buttSpell2))
        {
            PlayerController.ThrowSpell(2);
            CloseWheel();
        }
        else if (graphic.Equals(buttExit))
        {
            //Exit
            //Debug.Log("Exit");
            CloseWheel();
        }

    }

    private void CloseWheel()
    {
        UIManager.ShowSpellWheel(false);
        PlayerController.Instance.playerState = PlayerController.PlayerState.IdleState;
    }

}
