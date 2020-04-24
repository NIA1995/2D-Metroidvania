using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestTypeEnum
{
    KillMonster,
    ExchangeItem,
    TalkNPC,
}

public class QuestData : MonoBehaviour
{
    public int QuestCode;
    public int TargetObjectCount = 0;
    public int NowTargetCount = 0;

    public int QuestExp;
    public int QuestGold;

    public bool IsAccept = false;
    public bool IsClear = false;

    public QuestTypeEnum QuestType;

    public int ObjectCode;

    public string QuestName;

    public string[] QuestAcceptSentence;
    public string[] QuestClearSentence;

    public GameObject HudImage;

    private void OnMouseDown()
    {
        if(QuestSystem.Instance.DialougeGroup.alpha == 0 && !IsAccept && !IsClear)
        {
            QuestSystem.Instance.OnDialouge(this, QuestAcceptSentence);
        }
        else if(QuestSystem.Instance.DialougeGroup.alpha == 0 && IsAccept && !IsClear) 
        {
            QuestSystem.Instance.OnDialouge(this, QuestClearSentence);
        }
    }

    public void QuestAccept()
    {
        if(!IsAccept && !IsClear)
        {
            IsAccept = true;

            PlayerController.Instance.AcceptQuestList.Add(this);
        }
    }

    public void QuestClear()
    {
        if(TargetObjectCount <= NowTargetCount)
        {
            IsClear = true;
            IsAccept = false;

            PlayerController.Instance.Exp += QuestExp;
            PlayerController.Instance.Gold += QuestGold;

            PlayerController.Instance.AcceptQuestList.Remove(this);

            GameObject Hud = Instantiate(HudImage);
            Hud.GetComponent<HudText>().Alpha = Color.yellow;
            Hud.GetComponent<HudText>().TargetString = "Clear!\n" + "Get Gold : " + QuestGold + "\n" + "Get Exp : " + QuestExp;
            Hud.GetComponent<HudText>().transform.position = PlayerController.Instance.transform.position;


            enabled = false;
        }
        else
        {
            GameObject Hud = Instantiate(HudImage);
            Hud.GetComponent<HudText>().Alpha = Color.red;
            Hud.GetComponent<HudText>().TargetString = "Fail! \n" + QuestName + "\n" + NowTargetCount + " / " + TargetObjectCount;
            Hud.GetComponent<HudText>().transform.position = PlayerController.Instance.transform.position;

            return;
        }
    }
}
