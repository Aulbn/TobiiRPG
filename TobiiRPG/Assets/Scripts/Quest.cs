using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Quest", order = 1)]
public class Quest : ScriptableObject
{
    public string questName;
    [TextArea]
    public string questFeedback;
    [TextArea]
    public string questTip;
}
