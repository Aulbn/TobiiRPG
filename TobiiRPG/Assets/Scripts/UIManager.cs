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
    public Image enemyMarker;
    public float markerLifeTime = 1f;
    private float markerTimer = 0;
    private EnemyController target;
    public static Transform Target { get { return Instance.target == null ? null : Instance.target.transform; } }
    public Vector3 markerOffset;
    public Vector3 healthbarOffset;
    public SpellWheel spellWheel;
    public Image enemyHealthbar;
    public Image enemyHealthbarFill;
    [Header("Selection")]
    public Image selectionMarker;
    [Header("Player")]
    public GameObject walkStateImg;
    public GameObject idleStateImg;
    public Image healthbar;

    private void Awake()
    {
        Instance = this;
        graphicRaycaster = Instance.GetComponent<GraphicRaycaster>();
        eventSystem = Instance.GetComponent<EventSystem>();
    }

    private void Start()
    {
        SetStateSprite();
        spellWheel.gameObject.SetActive(false);
    }

    private void Update()
    {
        EnemyMarkerUpdate();
        healthbar.fillAmount = Mathf.Lerp(healthbar.fillAmount, PlayerController.CurrentHealth / PlayerController.Instance.maxHealth, Time.deltaTime * 5);
    }

    private void EnemyMarkerUpdate()
    {
        if (target == null || markerTimer <= 0)
        {
            enemyMarker.gameObject.SetActive(false);
            enemyHealthbar.gameObject.SetActive(false);
            if (target != null)
                enemyHealthbarFill.fillAmount = target.currentHealth / target.maxHealth;
            return;
        } else {
            markerTimer -= Time.deltaTime;
            Instance.enemyMarker.rectTransform.position = InsideScreenPos(EnemyMarkerPosition(), enemyMarker.rectTransform.sizeDelta.x / 2);
            Instance.enemyHealthbar.rectTransform.position = InsideScreenPos(Camera.main.WorldToScreenPoint(Instance.target.transform.position) + healthbarOffset, enemyHealthbar.rectTransform.sizeDelta.x / 2);
            enemyHealthbarFill.fillAmount = Mathf.Lerp(enemyHealthbarFill.fillAmount, target.currentHealth / target.maxHealth, Time.deltaTime * 5);
        }
    }

    public static Vector3 EnemyMarkerPosition()
    {
        if (Target == null)
            return Vector3.zero;
        return Camera.main.WorldToScreenPoint(Instance.target.transform.position) + Instance.markerOffset;
    }

    public static void ShowSpellWheel(bool show)
    {
        Instance.spellWheel.transform.position = Instance.enemyMarker.transform.position;
        Instance.spellWheel.gameObject.SetActive(show);
        Instance.ShowEnemyMarker(false);
    }

    private void ShowEnemyMarker(bool show)
    {
        markerTimer = show ? markerLifeTime : 0;
        enemyMarker.gameObject.SetActive(show);
        enemyHealthbar.gameObject.SetActive(show);
    }

    public static void TargetEnemy(Transform enemy)
    {
        Instance.ShowEnemyMarker(true);
        Instance.target = enemy.GetComponent<EnemyController>();
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

    public static Vector3 InsideScreenPos(Vector3 normalPosition, float radius)
    {
        return new Vector3(Mathf.Clamp(normalPosition.x, radius, Screen.width - radius), Mathf.Clamp(normalPosition.y, radius, Screen.height - radius));
    }

}
