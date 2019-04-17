using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class TobiiEventSystem : MonoBehaviour
{
    public Vector2 gaze;

    private GraphicRaycaster m_Raycaster;
    private PointerEventData m_PointerEventData;
    private EventSystem m_EventSystem;
    private Selectable currSel;

    void Start()
    {
        m_EventSystem = GetComponent<EventSystem>();
        m_Raycaster = FindObjectOfType<GraphicRaycaster>();
    }

    void Update()
    {
        NewUpdate();
    }

    private void NewUpdate()
    {
        if (GazeManager.IsConnected)
        {


            if (currSel == null)
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
            else if (currSel.GetComponent<TMP_InputField>() && EventSystem.current.currentSelectedGameObject != currSel && EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>())
            {
                currSel = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>();
            }

            gaze = new Vector2(TobiiAPI.GetGazePoint().Screen.x, (TobiiAPI.GetGazePoint().Screen.y - Screen.height) * (-1));

            m_PointerEventData = new PointerEventData(m_EventSystem);
            m_PointerEventData.position = TobiiAPI.GetGazePoint().Screen;
            List<RaycastResult> results = new List<RaycastResult>();

            m_Raycaster.Raycast(m_PointerEventData, results);

            //if (currSel != null && !currSel.GetComponent<TMP_InputField>())
            //{
            //    print("Nullar för att " + currSel.gameObject.name + "är inte ett inputfield");
            //    currSel = null;
            //}

            foreach (RaycastResult result in results)
            {
                if (result.gameObject.GetComponent<Selectable>())
                {
                    print("här?");
                    if (currSel == null || !currSel.GetComponent<TMP_InputField>())
                    {
                        print("men inte här?");
                        result.gameObject.GetComponent<Selectable>().Select();
                        currSel = result.gameObject.GetComponent<Selectable>();
                    }
                    break;
                }
                if (!result.gameObject.GetComponent<TextMeshProUGUI>())
                    break;
            }
        }
    }
}

