using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class GazeButton : MonoBehaviour
{
    [Header("Tobii")]
    public Image targetFillGraphic;
    public Gradient fillGradient;
    public float preDwellTime = 1;
    public float dwellTime = 1;
    public float graceTime = 0.5f;

    private Button button;
    private RectTransform buttonRect;
    private float dwellTimer = 0;
    private float graceTimer = 0;

    private void Start()
    {
        button = GetComponent<Button>();
        buttonRect = button.GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (dwellTimer >= preDwellTime)
            targetFillGraphic.fillAmount = (dwellTimer - preDwellTime) / dwellTime;
        else
            targetFillGraphic.fillAmount = 0;
        targetFillGraphic.color = fillGradient.Evaluate((dwellTimer - preDwellTime) / dwellTime);
        if (isHovering())
        {
            button.Select();
            dwellTimer += Time.unscaledDeltaTime;
            if ((dwellTimer - preDwellTime) >= dwellTime)
            {
                dwellTimer = 0;
                button.onClick.Invoke();
            }
        }
        else
        {
            graceTimer += Time.unscaledDeltaTime;
            if (graceTimer >= graceTime)
            {
                dwellTimer = 0;
                graceTimer = 0;
            }
        }
    }

    private bool isHovering()
    {
        Vector2 gaze = GazeManager.GazePosition;
        Vector2 radius = new Vector2(buttonRect.rect.size.x / 2, buttonRect.rect.size.y / 2);
        Vector2 buttonPos = buttonRect.position;
        if (gaze.x > (buttonPos.x - radius.x) && gaze.x < (buttonPos.x + radius.x))
        {
            if (gaze.y > (buttonPos.y - radius.y) && gaze.y < (buttonPos.y + radius.y))
            {
                return true;
            }
        }
        return false;
    }
}
