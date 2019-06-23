using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestDisplay : MonoBehaviour
{
    public static QuestDisplay Instance;
    public Text qName, qFeedbackNmbr, qFeedbackText, qTip;
    public Color successColor;
    public float preTransitionTime;
    public float transitionTime;

    public static bool isTransitioning = false;
    private void Awake()
    {
        Instance = this;
    }

    public static void SetQuestInfo(string qname, string feedbackNmbr, string feedbackText, string tip)
    {
        Instance.qName.text = qname;
        Instance.qFeedbackNmbr.text = feedbackNmbr;
        Instance.qFeedbackText.text = feedbackText;
        Instance.qTip.text = tip;
    }

    public static IEnumerator QuestFadeout()
    {
        isTransitioning = true;
        Instance.qFeedbackNmbr.color = Instance.successColor;
        Instance.qFeedbackText.color = Instance.successColor;
        yield return new WaitForSeconds(Instance.preTransitionTime);

        Instance.qName.CrossFadeAlpha(0, Instance.transitionTime, true);
        Instance.qFeedbackNmbr.CrossFadeAlpha(0, Instance.transitionTime, true);
        Instance.qFeedbackText.CrossFadeAlpha(0, Instance.transitionTime, true);
        Instance.qTip.CrossFadeAlpha(0, Instance.transitionTime, true);
        yield return new WaitForSeconds(Instance.transitionTime);

        QuestManager.NewQuest();
    }

    public static void QuestFadein()
    {
        Instance.qFeedbackNmbr.color = Color.white;
        Instance.qFeedbackText.color = Color.white;
        Instance.qName.CrossFadeAlpha(1, Instance.transitionTime, true);
        Instance.qFeedbackNmbr.CrossFadeAlpha(1, Instance.transitionTime, true);
        Instance.qFeedbackText.CrossFadeAlpha(1, Instance.transitionTime, true);
        Instance.qTip.CrossFadeAlpha(1, Instance.transitionTime, true);
        isTransitioning = false;
    }

    public static void SetQuestFeedback(string feedbackNmbr)
    {
        Instance.qFeedbackNmbr.text = feedbackNmbr;
    }

}
