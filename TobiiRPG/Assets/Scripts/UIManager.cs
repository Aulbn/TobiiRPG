using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public static GraphicRaycaster graphicRaycaster;
    public static EventSystem eventSystem;
    [Header("Enemy Targeting")]
    public Image targetMarker;
    public float markerLifeTime = 1f;
    [HideInInspector]
    public float markerTimer = 0;
    [HideInInspector]
    public Target target;
    public static Transform Target { get { return Instance.target == null ? null : Instance.target.transform; } }
    public Vector3 markerOffset;
    public Vector3 healthbarOffset;
    public SpellWheel spellWheel;
    public StateWheel stateWheel;
    public Image enemyHealthbar;
    public Image enemyHealthbarFill;
    [Header("Selection")]
    public Image selectionMarker;
    [Header("Player")]
    public Image healthbarFill;
    public Gradient healthbarColor;
    [Header("State")]
    public GameObject walkStateImg;
    public GameObject idleStateImg;
    public Image stateFrame, stateDisplay;
    public Vector3 stateDisplayToggleOffset;
    public Color walkColor, combatColor;
    public GameObject pauseScreen;
    public GameObject deathScreen;
    public GameObject winScreen;

    private void Awake()
    {
        Instance = this;
        graphicRaycaster = Instance.GetComponent<GraphicRaycaster>();
        eventSystem = Instance.GetComponent<EventSystem>();
        //DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SetStateSprite();
        spellWheel.gameObject.SetActive(false);
        GazeManager.OnEyeGestureOutsideScreen += () => TogglePauseScreen(true);
    }

    private void Update()
    {
        TargetMarkerUpdate();
        healthbarFill.fillAmount = Mathf.Lerp(healthbarFill.fillAmount, PlayerController.CurrentHealth / PlayerController.Instance.maxHealth, Time.unscaledDeltaTime * 5);
        healthbarFill.color = healthbarColor.Evaluate(healthbarFill.fillAmount);
        stateFrame.color = PlayerController.Instance.playerState == PlayerController.PlayerState.MoveState ? walkColor : combatColor;
    }

    private void TargetMarkerUpdate()
    {
        if (target == null || markerTimer <= 0 || (TargetType().IsSubclassOf(typeof(Killable)) && ((Killable)target).HealthPercentage == 0))
        {
            targetMarker.gameObject.SetActive(false);
            enemyHealthbar.gameObject.SetActive(false);
            //if (target != null && TargetType() == typeof(Killable))
            //    enemyHealthbarFill.fillAmount = ((Killable)target).HealthPercentage;
            //return;
        }
        else
        {
            markerTimer -= Time.deltaTime;
            Instance.targetMarker.rectTransform.position = InsideScreenPos(TargetMarkerPosition(), targetMarker.rectTransform.sizeDelta.x / 2);
            if (TargetType() == typeof(EnemyController))
            {
                enemyHealthbar.gameObject.SetActive(true);
                enemyHealthbarFill.fillAmount = Mathf.Lerp(enemyHealthbarFill.fillAmount, ((Killable)target).HealthPercentage, Time.deltaTime * 5);
                Instance.enemyHealthbar.rectTransform.position = InsideScreenPos(Camera.main.WorldToScreenPoint(Instance.target.transform.position) + healthbarOffset, enemyHealthbar.rectTransform.sizeDelta.x / 2);
            }
        }
    }

    public static Vector3 TargetMarkerPosition()
    {
        if (Target == null)
            return Vector3.zero;
        if (TargetType().IsSubclassOf(typeof(Killable)))
            return Camera.main.WorldToScreenPoint(Instance.target.transform.position) + Instance.markerOffset;
        else
            return Instance.target.GetComponent<RectTransform>().position + Instance.stateDisplayToggleOffset * (Instance.stateDisplay.rectTransform.sizeDelta.x / 2);
    }

    public static void ShowSpellWheel(bool show)
    {
        Instance.spellWheel.transform.position = Instance.targetMarker.transform.position;
        Instance.spellWheel.gameObject.SetActive(show);
        ShowTargetMarker(false);
    }

    public static void ShowStateWheel(bool show)
    {
        Instance.stateWheel.transform.position = Instance.targetMarker.transform.position;
        Instance.stateWheel.gameObject.SetActive(show);
        ShowTargetMarker(false);
    }

    public static void ShowTargetMarker(bool show)
    {
        Instance.markerTimer = show ? Instance.markerLifeTime : 0;
        Instance.targetMarker.gameObject.SetActive(show);
        if (Instance.target != null)
            Instance.enemyHealthbar.gameObject.SetActive(show && TargetType() == typeof(Killable));
    }

    public static void SetTarget(Transform targ)
    {
        ShowTargetMarker(true);
        Instance.target = targ.GetComponent<Target>();
        Instance.selectionMarker.fillAmount = 0;
        if (TargetType() == typeof(EnemyController))
            Instance.enemyHealthbarFill.fillAmount = ((Killable)Instance.target).HealthPercentage;
    }

    public static void SetSelectionMarker (float value, Vector3 position)
    {
        Instance.selectionMarker.gameObject.SetActive(value > 0 ? true : false);
        Instance.selectionMarker.fillAmount = value;
        Instance.selectionMarker.rectTransform.position = Camera.main.WorldToScreenPoint (position);
    }

    public static void SetStateSprite()
    {
        bool isWalking = PlayerController.Instance.playerState == PlayerController.PlayerState.MoveState;
        Instance.walkStateImg.SetActive(isWalking ? true : false);
        Instance.idleStateImg.SetActive(isWalking ? false : true);
    }

    public static void ShowDeathScreen()
    {
        Time.timeScale = 0;
        Instance.deathScreen.SetActive(true);
    }

    public void TogglePauseScreen(bool value)
    {
        pauseScreen.SetActive(value);
        GameManager.PauseGame(value);
    }

    public void RestartGame()
    {
        deathScreen.SetActive(false);
        GameManager.PauseGame(false);
        GameManager.RestartGame();
    }
    public void QuitGame()
    {
        GameManager.QuitGame();
    }

    public static void ShowWinScreen(bool value)
    {
        Instance.winScreen.SetActive(value);
    }

    public void Respawn()
    {
        deathScreen.SetActive(false);
        GameManager.Respawn();
    }

    public static Vector3 InsideScreenPos(Vector3 normalPosition, float radius)
    {
        return new Vector3(Mathf.Clamp(normalPosition.x, radius, Screen.width - radius), Mathf.Clamp(normalPosition.y, radius, Screen.height - radius));
    }

    public static System.Type TargetType()
    {
        return Instance.target.GetType();
    }

}
