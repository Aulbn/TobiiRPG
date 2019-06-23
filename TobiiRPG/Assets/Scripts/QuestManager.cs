using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    private int currentQuest = 0;
    public float transitionTime = 4f;

    [Header("Quest 1")]
    public Quest quest1;
    public TriggerGateControl triggerGate;
    [Header("Quest 2")]
    public Quest quest2;
    public TargetPractice targetPractice;
    [Header("Quest 3")]
    public Quest quest3;
    private int startEnemies;
    [Header("Quest 4")]    
    public Quest quest4;
    public BoxCollider goalTrigger;
    public Animator gateAnimator;
    public GameObject[] braziers;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        startEnemies = EnemyController.LivingEnemies;
        NewQuest();
    }

    public void Update()
    {
        switch (currentQuest)
        {
            case 1:
                QuestDisplay.SetQuestFeedback(triggerGate.TriggeredPlates + "/" + 2);
                if (triggerGate.isUnlocked && !QuestDisplay.isTransitioning)
                    EndQuest();
                break;
            case 2:
                if (targetPractice.isUnlocked && !QuestDisplay.isTransitioning)
                {
                    QuestDisplay.SetQuestFeedback("4/4");
                    EndQuest();
                } else
                {
                    QuestDisplay.SetQuestFeedback(targetPractice.DeadTargets + "/" + 4);
                }
                break;
            case 3:
                QuestDisplay.SetQuestFeedback(EnemyController.LivingEnemies + "/" + startEnemies);
                if (EnemyController.LivingEnemies == 0 && !QuestDisplay.isTransitioning)
                {
                    gateAnimator.SetBool("Open", true);
                    LitBraziers();
                    EndQuest();
                }
                break;
            case 4:
                if (goalTrigger.bounds.Contains(PlayerController.Instance.transform.position))
                {
                    GameManager.WinGame();
                }
                break;
            default:
                break;

        }
    }

    private void EndQuest()
    {
        StartCoroutine(QuestDisplay.QuestFadeout());
    }

    private void LitBraziers()
    {
        for (int i = 0; i < braziers.Length; i++)
        {
            braziers[i].GetComponent<Brazier_control>().SetIsLit(true);
        }

    }

    public static void NewQuest()
    {
        if (Instance.currentQuest < 4)
        {
            ++Instance.currentQuest;
            switch (Instance.currentQuest)
            {
                case 1:
                    QuestDisplay.SetQuestInfo(Instance.quest1.questName, "", Instance.quest1.questFeedback, Instance.quest1.questTip);
                    break;
                case 2:
                    QuestDisplay.SetQuestInfo(Instance.quest2.questName, "", Instance.quest2.questFeedback, Instance.quest2.questTip);
                    break;
                case 3:
                    QuestDisplay.SetQuestInfo(Instance.quest3.questName, "", Instance.quest3.questFeedback, Instance.quest3.questTip);
                    break;
                case 4:
                    QuestDisplay.SetQuestInfo(Instance.quest4.questName, "", Instance.quest4.questFeedback, Instance.quest4.questTip);
                    break;
                default:
                    break;

            }
            QuestDisplay.QuestFadein();
        }
    }

}
